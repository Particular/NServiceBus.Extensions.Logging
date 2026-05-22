namespace NServiceBus.Extensions.Logging.NLog.AcceptanceTests;

using System.Threading.Tasks;
using global::NLog.Config;
using global::NLog.Extensions.Logging;
using global::NLog.Targets;
using NUnit.Framework;
using NServiceBus.AcceptanceTesting;
using NServiceBus.AcceptanceTests;
using NServiceBus.AcceptanceTests.EndpointTemplates;
using NServiceBus.Logging;
using NLogLogManager = global::NLog.LogManager;
using NsbLogManager = NServiceBus.Logging.LogManager;

[NonParallelizable]
public class When_using_nlog_extension_logger : NServiceBusAcceptanceTest
{
    [Test]
    public async Task Should_log_nsb_logs_through_nlog()
    {
        var memoryTarget = new MemoryTarget();
        var config = new LoggingConfiguration();
        config.AddRuleForAllLevels(memoryTarget);
        NLogLogManager.Configuration = config;

        await using var nlogLoggerFactory = new NLogLoggerFactory();

#pragma warning disable CS0618 // ExtensionsLoggerFactory is deprecated; test exercises legacy behavior intentionally
        NsbLogManager.UseFactory(new ExtensionsLoggerFactory(nlogLoggerFactory));
#pragma warning restore CS0618

        await Scenario.Define<Context>()
            .WithEndpoint<EndpointUsingBridge>()
            .Done(c => c.EndpointsStarted)
            .Run();

        Assert.That(memoryTarget.Logs, Is.Not.Empty);
    }

    [TearDown]
    public void Teardown()
    {
#pragma warning disable CS0618
        NsbLogManager.Use<DefaultFactory>();
#pragma warning restore CS0618
    }

    class Context : ScenarioContext;

    class EndpointUsingBridge : EndpointConfigurationBuilder
    {
        public EndpointUsingBridge() => EndpointSetup<DefaultServer>();
    }
}