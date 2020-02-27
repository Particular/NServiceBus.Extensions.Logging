namespace NServiceBus
{
    using Microsoft.Extensions.Logging;
    using NServiceBus.Configuration.AdvancedExtensibility;

    /// <summary>
    ///
    /// </summary>
    public static class EndpointConfigurationExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="endpointConfiguration"></param>
        /// <param name="loggerFactory"></param>
        public static void UseLogger(this EndpointConfiguration endpointConfiguration,
            ILoggerFactory loggerFactory)
        {
            //TODO: Could we do something around allowing the MS ILogger to be injected?
            endpointConfiguration.GetSettings().Set(loggerFactory);
        }
    }
}
