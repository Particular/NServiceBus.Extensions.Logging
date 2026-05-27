namespace NServiceBus.Extensions.Logging.Tests
{
    using Logging;
    using NUnit.Framework;
    using Particular.Approvals;
    using PublicApiGenerator;

    [TestFixture]
    public class APIApprovals
    {
        [Test]
        public void Approve_NServiceBusExtensionsLogging()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var publicApi = typeof(ExtensionsLoggerFactory).Assembly.GeneratePublicApi(new ApiGeneratorOptions
#pragma warning restore CS0618 // Type or member is obsolete
            {
                ExcludeAttributes =
                [
                    "System.Runtime.Versioning.TargetFrameworkAttribute",
                    "System.Reflection.AssemblyMetadataAttribute",
                ]
            });
            Approver.Verify(publicApi);
        }
    }
}
