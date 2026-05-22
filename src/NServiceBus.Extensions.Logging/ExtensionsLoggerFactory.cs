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
        Message = "The logging bridge is no longer required. NServiceBus natively uses Microsoft.Extensions.Logging. When using 'AddNServiceBusEndpoint', configure logging on the host builder. For self-hosting with 'Endpoint.Start', use 'RegisterComponents(s => s.AddLogging(builder => builder.AddNLog() / builder.AddSerilog() / etc.))' instead and remove this package reference. 'RegisterComponents' is also deprecated but CS0618 may be suppressed as a migration step until all service registrations are moved to a host-managed service collection.",
        TreatAsErrorFromVersion = "5",
        RemoveInVersion = "6")]
    [Obsolete("The logging bridge is no longer required. NServiceBus natively uses Microsoft.Extensions.Logging. When using 'AddNServiceBusEndpoint', configure logging on the host builder. For self-hosting with 'Endpoint.Start', use 'RegisterComponents(s => s.AddLogging(builder => builder.AddNLog() / builder.AddSerilog() / etc.))' instead and remove this package reference. 'RegisterComponents' is also deprecated but CS0618 may be suppressed as a migration step until all service registrations are moved to a host-managed service collection. Will be treated as an error from version 5.0.0. Will be removed in version 6.0.0.", false)]
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
