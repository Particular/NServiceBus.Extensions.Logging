namespace NServiceBus
{
    using NServiceBus.Extensions.Logging;
    using NServiceBus.Features;
    using NServiceBus.Logging;

    class MSLoggingFeature : Feature
    {
        public MSLoggingFeature()
        {
            EnableByDefault();
        }
        protected override void Setup(FeatureConfigurationContext context)
        {
            var loggerFactory = context.Settings.Get<Microsoft.Extensions.Logging.ILoggerFactory>();

            LogManager.UseFactory(new ExtensionsLoggerFactory(loggerFactory));
        }
    }
}
