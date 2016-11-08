using System;
using System.Linq;

namespace CrossDomainAssemblyMetadataComparer.Core
{
    public sealed class MetadataComparisonParameters
    {
        public static readonly MetadataComparisonParameters Default = new MetadataComparisonParameters();

        private MetadataComparisonParameters()
        {
            // Nothing to do
        }
    }
}