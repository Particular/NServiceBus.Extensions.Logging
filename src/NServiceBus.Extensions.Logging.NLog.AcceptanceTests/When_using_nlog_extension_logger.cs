namespace NServiceBus.Extensions.DependencyInjection.AcceptanceTests
{
    using System;
    using System.Threading.Tasks;
    using Logging;
    using NLog;
    using NLog.Extensions.Logging;
    using NUnit.Framework;
    //using NsbLogManager = NServiceBus;

    public class When_using_nlog_extension_logger
    {
        [Test]
        public async Task Should_log_nsb_logs()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var memoryTarget = new NLog.Targets.MemoryTarget();
            config.AddRuleForAllLevels(memoryTarget);
            LogManager.Configuration = config;

            //NsbLogManager.Use<ExtensionLogging<NLogLoggerFactory>>();

            var endpointConfiguration = new EndpointConfiguration("LoggingTests");

            //this is the new API
            endpointConfiguration.UseLogger(new NLogLoggerFactory());

            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            var endpoint = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            try
            {
                foreach (var l in memoryTarget.Logs)
                {
                    Console.WriteLine(l);
                }

                Assert.IsNotEmpty(memoryTarget.Logs);

            }
            finally
            {
                await endpoint.Stop()
                    .ConfigureAwait(false);
            }
        }
    }
}