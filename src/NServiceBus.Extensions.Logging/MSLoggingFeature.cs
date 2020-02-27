namespace NServiceBus
{
    using NServiceBus.Extensions.Logging;
    using NServiceBus.Features;
    using NServiceBus.Logging;
    using System;
    using System.Collections.Generic;

    class MSLoggingFeature : Feature
    {
        static MSLoggingFeature()
        {
            tempLogBuffer = new TempLogBuffer();
            LogManager.UseFactory(tempLogBuffer);
        }
        public MSLoggingFeature()
        {
            EnableByDefault();
        }
        protected override void Setup(FeatureConfigurationContext context)
        {
            //TODO: Throw if other logger than TempLogBuffer is used
            //TODO: Check that the user have called .UseLogger
            

            var loggerFactory = context.Settings.Get<Microsoft.Extensions.Logging.ILoggerFactory>();

            var factory = new ExtensionsLoggerFactory(loggerFactory);
            LogManager.UseFactory(new ExtensionsLoggerFactory(loggerFactory));

            //Forward buffered logs
            foreach (var i in tempLogBuffer.factoryInvocations)
            {
                if (i.Name != null)
                {
                    var logger = factory.GetLogger(i.Name);

                    foreach (var a in i.LogInvocations.Actions)
                    {
                        a(logger);
                    }
                }

                if (i.Type != null)
                {
                    var logger = factory.GetLogger(i.Type);

                    foreach (var a in i.LogInvocations.Actions)
                    {
                        a(logger);
                    }
                }
            }
        }

        static TempLogBuffer tempLogBuffer;
    }

    class TempLogBuffer : ILoggerFactory
    {
        public ILog GetLogger(Type type)
        {
            var invocation = new LogFactoryInvocation
            {
                Type = type
            };

            factoryInvocations.Add(invocation);

            return invocation.LogInvocations;
        }

        public ILog GetLogger(string name)
        {
            var invocation = new LogFactoryInvocation
            {
                Name = name
            };

            factoryInvocations.Add(invocation);

            return invocation.LogInvocations;
        }

        public List<LogFactoryInvocation> factoryInvocations = new List<LogFactoryInvocation>();

        public class LogFactoryInvocation
        {
            public Type Type;
            public string Name;
            public InMemoryLogBuffer LogInvocations = new InMemoryLogBuffer();
        }

        public class InMemoryLogBuffer : ILog
        {
            public List<Action<ILog>> Actions { get; } = new List<Action<ILog>>();

            public bool IsDebugEnabled => true;

            public bool IsInfoEnabled => true;

            public bool IsWarnEnabled => true;

            public bool IsErrorEnabled => true;

            public bool IsFatalEnabled => true;


            public void Debug(string message)
            {
                Actions.Add(l => l.Debug(message));
            }

            public void Debug(string message, Exception exception)
            {
                Actions.Add(l => l.Debug(message, exception));
            }

            public void DebugFormat(string format, params object[] args)
            {
                Actions.Add(l => l.DebugFormat(format, args));
            }

            public void Error(string message)
            {
                Actions.Add(l => l.Error(message));
            }

            public void Error(string message, Exception exception)
            {
                Actions.Add(l => l.Error(message, exception));
            }

            public void ErrorFormat(string format, params object[] args)
            {
                Actions.Add(l => l.ErrorFormat(format, args));

            }

            public void Fatal(string message)
            {
                Actions.Add(l => l.Fatal(message));
            }

            public void Fatal(string message, Exception exception)
            {
                Actions.Add(l => l.Fatal(message, exception));
            }

            public void FatalFormat(string format, params object[] args)
            {
                Actions.Add(l => l.FatalFormat(format, args));
            }

            public void Info(string message)
            {
                Actions.Add(l => l.Info(message));
            }

            public void Info(string message, Exception exception)
            {
                Actions.Add(l => l.Info(message, exception));
            }

            public void InfoFormat(string format, params object[] args)
            {
                Actions.Add(l => l.InfoFormat(format, args));
            }

            public void Warn(string message)
            {
                Actions.Add(l => l.Warn(message));
            }

            public void Warn(string message, Exception exception)
            {
                Actions.Add(l => l.Warn(message, exception));
            }

            public void WarnFormat(string format, params object[] args)
            {
                Actions.Add(l => l.WarnFormat(format, args));
            }
        }
    }
}
