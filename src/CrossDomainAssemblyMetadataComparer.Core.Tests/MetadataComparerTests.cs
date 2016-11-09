using System;
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

            var comparandAssemblyReference =
                new FileSystemAssemblyReference(
                    @"C:\Src\Tfs\MobiControl\DEV\MobiControl\MobiControl .NET\Entities\bin\Debug\Soti.MobiControl.Entities.dll");

            var comparer = new MetadataComparer(examineeAssemblyReference, comparandAssemblyReference);
            var comparisonResult = comparer.Compare();
            Assert.That(comparisonResult, Is.Not.Null);
        }
    }
}