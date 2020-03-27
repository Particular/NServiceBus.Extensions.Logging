namespace NServiceBus.Extensions.Logging
{
    using System;
    using NServiceBus.Logging;

    /// <summary>
    /// Usage:
    ///       ILoggerFactory extensionsLoggingFactory = ...;
    ///       LogManager.UseFactory(new ExtensionsLoggerFactory(extensionsLoggingFactory));
    /// </summary>
    public class ExtensionsLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// Creates an NServiceBus ILoggerFactory instance based on Microsoft.Extensions.Logging.
        /// </summary>
        /// <param name="loggerFactory">An initialized <see cref="Microsoft.Extensions.Logging.ILoggerFactory"/> instance.</param>
        public ExtensionsLoggerFactory(Microsoft.Extensions.Logging.ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <inheritdoc />
        public ILog GetLogger(Type type)
        {
            return new ExtensionsLogger(loggerFactory.CreateLogger(type.FullName));
        }

        /// <inheritdoc />
        public ILog GetLogger(string name)
        {
            return new ExtensionsLogger(loggerFactory.CreateLogger(name));
        }

        Microsoft.Extensions.Logging.ILoggerFactory loggerFactory;
    }
}
