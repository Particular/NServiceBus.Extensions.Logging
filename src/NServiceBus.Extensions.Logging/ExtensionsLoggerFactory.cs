namespace NServiceBus.Extensions.Logging
{
    using System;
    using NServiceBus.Logging;
    using Particular.Obsoletes;

    /// <summary>
    /// Usage:
    ///       ILoggerFactory extensionsLoggingFactory = ...;
    ///       LogManager.UseFactory(new ExtensionsLoggerFactory(extensionsLoggingFactory));
    /// </summary>
    [ObsoleteMetadata(
        Message = "The logging bridge is no longer required. NServiceBus now natively uses Microsoft.Extensions.Logging when hosting endpoints with AddNServiceBusEndpoint.",
        TreatAsErrorFromVersion = "5",
        RemoveInVersion = "6")]
    [Obsolete("The logging bridge is no longer required. NServiceBus now natively uses Microsoft.Extensions.Logging when hosting endpoints with AddNServiceBusEndpoint. Will be treated as an error from version 5.0.0. Will be removed in version 6.0.0.", false)]
    public class ExtensionsLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// Creates an NServiceBus ILoggerFactory instance based on Microsoft.Extensions.Logging.
        /// </summary>
        /// <param name="loggerFactory">An initialized <see cref="Microsoft.Extensions.Logging.ILoggerFactory"/> instance.</param>
        public ExtensionsLoggerFactory(Microsoft.Extensions.Logging.ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(loggerFactory);

            this.loggerFactory = loggerFactory;
        }

        /// <inheritdoc />
        public ILog GetLogger(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            return new ExtensionsLogger(loggerFactory.CreateLogger(type.FullName));
        }

        /// <inheritdoc />
        public ILog GetLogger(string name) => new ExtensionsLogger(loggerFactory.CreateLogger(name));

        readonly Microsoft.Extensions.Logging.ILoggerFactory loggerFactory;
    }
}
