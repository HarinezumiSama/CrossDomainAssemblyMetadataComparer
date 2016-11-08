namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public sealed class MetadataComparisonParameters
    {
        public static readonly MetadataComparisonParameters Default = new MetadataComparisonParameters();

        private MetadataComparisonParameters()
        {
            CreateTypeNameMatcher = candidateTypes => new DefaultTypeNameMatcher(candidateTypes);
        }

        public TypeNameMatcherCreator CreateTypeNameMatcher
        {
            get;
        }
    }
}