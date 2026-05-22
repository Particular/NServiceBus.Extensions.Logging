#pragma warning disable CS0618 // Type or member is obsolete
namespace NServiceBus.Extensions.Logging.NLog.AcceptanceTests;

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Configuration.AdvancedExtensibility;
using global::NLog.Config;
using global::NLog.Extensions.Logging;
using global::NLog.Targets;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NServiceBus.AcceptanceTesting;
using NServiceBus.AcceptanceTests;
using NServiceBus.AcceptanceTests.EndpointTemplates;
using Settings;

[NonParallelizable]
public class When_using_nlog_with_register_components : NServiceBusAcceptanceTest
{
    [Test]
    public async Task Should_log_nsb_logs_through_nlog()
    {
        var memoryTarget = new MemoryTarget();
        var config = new LoggingConfiguration();
        config.AddRuleForAllLevels(memoryTarget);

        await Scenario.Define<Context>()
            .WithEndpoint<EndpointWithRegisterComponents>(b =>
            {
                b.CustomConfig(c => c.RegisterComponents(s => s.AddLogging(builder => builder.AddNLog(config))));
                b.ToCreateInstance(
                    (_, configuration) =>
                    {
                        // The acceptance test infrastructure populates settings with the adapter which then makes RegisterComponent to be rerouted to the adapter
                        // in self-hosting scenarios this is never the case as the adapter is only used for multi-endpoint hosting, so we need to remove the adapter
                        // from settings to properly test the scenario but it allows us to use the acceptance test infrastructure to create the endpoint instance and
                        // start it which is required to properly test the scenario
                        RemoveOverride(configuration.GetSettings(), "NServiceBus.KeyedServiceCollectionAdapter");
                        return Endpoint.Create(configuration);
                    },
                    async (startableEndpoint, _, ct) =>
                    {
                        var endpoint = await startableEndpoint.Start(ct);
                        return endpoint.Stop;
                    });
            })
            .Done(c => c.EndpointsStarted)
            .Run();

        Assert.That(memoryTarget.Logs, Is.Not.Empty);
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "Overrides")]
    static extern ref ConcurrentDictionary<string, object> GetOverrides(SettingsHolder settingsHolder);

    static void RemoveOverride(SettingsHolder settingsHolder, string key) => GetOverrides(settingsHolder).TryRemove(key, out _);

    class Context : ScenarioContext;

    class EndpointWithRegisterComponents : EndpointConfigurationBuilder
    {
        public EndpointWithRegisterComponents() => EndpointSetup<DefaultServer>();
    }
}
#pragma warning restore CS0618
