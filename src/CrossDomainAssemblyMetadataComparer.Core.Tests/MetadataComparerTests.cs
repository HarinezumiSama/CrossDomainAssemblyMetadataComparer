using System.Collections.Generic;
using System.Linq;
using CrossDomainAssemblyMetadataComparer.Core.Model;
using NUnit.Framework;

namespace CrossDomainAssemblyMetadataComparer.Core.Tests
{
    [TestFixture]
    public sealed class MetadataComparerTests
    {
        [Test]
        public void TestProofOfConceptScenario()
        {
            var examineeAssemblyReference =
                new FileSystemAssemblyReference(
                    @"C:\Src\Tfs\MobiControl\Common\DeploymentServerExtensions.Facade\v1\DeploymentServerExtensions.Facade\bin\Debug\Soti.MobiControl.DeploymentServerExtensions.Facade.dll");

            var comparandAssemblyPaths = new[]
            {
                @"C:\Src\Tfs\MobiControl\DEV\MobiControl\Lib\Soti.MobiControl.Entities.dll",
                @"C:\Src\Tfs\MobiControl\DEV\MobiControl\Lib\Soti.MobiControl.Api.dll",
                @"C:\Src\Tfs\MobiControl\DEV\MobiControl\Lib\Soti.MobiControl.CoreFeatures.dll",
                @"C:\Src\Tfs\MobiControl\DEV\MobiControl\Lib\Soti.MobiControl.DeviceConfiguration.dll",
                @"C:\Src\Tfs\MobiControl\DEV\MobiControl\Lib\Soti.MobiControl.Entities.Providers.dll",
                @"C:\Src\Tfs\MobiControl\DEV\MobiControl\Lib\Soti.MobiControl.WindowsModern.Service.dll",
                @"C:\Src\Tfs\MobiControl\DEV\MobiControl\Lib\Soti.MobiControl.DeploymentServerExtensions.dll"
            };

            var comparandAssemblyReferences = comparandAssemblyPaths
                .Select(path => new FileSystemAssemblyReference(path))
                .ToArray();

            var dependencyReferences = new DependencyReference[]
            {
                new DirectoryDependencyReference(@"C:\Src\Tfs\MobiControl\DEV\MobiControl\Packages", true)
            };

            var parameters = new MetadataComparisonParameters(dependencyReferences: dependencyReferences);

            var comparer = new MetadataComparer(examineeAssemblyReference, comparandAssemblyReferences, parameters);
            var comparisonResult = comparer.Compare();
            Assert.That(comparisonResult, Is.Not.Null);
        }
    }
}