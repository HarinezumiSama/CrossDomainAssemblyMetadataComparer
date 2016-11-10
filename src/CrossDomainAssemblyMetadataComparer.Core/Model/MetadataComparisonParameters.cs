using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public sealed class MetadataComparisonParameters
    {
        public static readonly MetadataComparisonParameters Default = new MetadataComparisonParameters();

        private static readonly ReadOnlyCollection<DependencyReference> EmptyDependencyReferences =
            new ReadOnlyCollection<DependencyReference>(new DependencyReference[0]);

        public MetadataComparisonParameters(
            [CanBeNull] TypeNameMatcherCreator createTypeNameMatcher = null,
            [CanBeNull] ICollection<DependencyReference> dependencyReferences = null)
        {
            if (dependencyReferences != null && dependencyReferences.Any(item => item == null))
            {
                throw new ArgumentException(@"The collection contains a null element.", nameof(dependencyReferences));
            }

            CreateTypeNameMatcher = createTypeNameMatcher
                ?? (candidateTypes => new DefaultTypeNameMatcher(candidateTypes));

            DependencyReferences = dependencyReferences?.ToArray().AsReadOnly() ?? EmptyDependencyReferences;
        }

        public TypeNameMatcherCreator CreateTypeNameMatcher
        {
            get;
        }

        public ReadOnlyCollection<DependencyReference> DependencyReferences
        {
            get;
        }
    }
}