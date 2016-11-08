using System;
using System.Collections.Generic;
using System.Linq;
using CrossDomainAssemblyMetadataComparer.Core.Model;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core
{
    public sealed class MetadataComparer
    {
        public MetadataComparer(
            [NotNull] AssemblyReference examineeAssemblyReference,
            [NotNull] AssemblyReference comparandAssemblyReference,
            [NotNull] MetadataComparisonParameters parameters)
        {
            if (examineeAssemblyReference == null)
            {
                throw new ArgumentNullException(nameof(examineeAssemblyReference));
            }

            if (comparandAssemblyReference == null)
            {
                throw new ArgumentNullException(nameof(comparandAssemblyReference));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            ExamineeAssemblyReference = examineeAssemblyReference;
            ComparandAssemblyReference = comparandAssemblyReference;
            Parameters = parameters;
        }

        public MetadataComparer(
            [NotNull] AssemblyReference examineeAssemblyReference,
            [NotNull] AssemblyReference comparandAssemblyReference)
            : this(examineeAssemblyReference, comparandAssemblyReference, MetadataComparisonParameters.Default)
        {
            // Nothing to do
        }

        [NotNull]
        public AssemblyReference ExamineeAssemblyReference
        {
            get;
        }

        [NotNull]
        public AssemblyReference ComparandAssemblyReference
        {
            get;
        }

        [NotNull]
        public MetadataComparisonParameters Parameters
        {
            get;
        }

        public MetadataComparisonResult Compare()
        {
            var examineeAssembly = ExamineeAssemblyReference.Load();
            var comparandAssembly = ComparandAssemblyReference.Load();

            var examineeTypes = examineeAssembly.GetTypes();
            var comparandTypes = comparandAssembly.GetTypes();

            var enumComparisonResult = ProcessEnums(examineeTypes, comparandTypes);

            return new MetadataComparisonResult(enumComparisonResult);
        }

        private EnumComparisonResult ProcessEnums(
            [NotNull] ICollection<Type> examineeTypes,
            [NotNull] ICollection<Type> comparandTypes)
        {
            var examineeEnums = examineeTypes.GetEnumTypes();
            var comparandEnums = comparandTypes.GetEnumTypes();

            var typeNameMatcher = Parameters.CreateTypeNameMatcher(comparandEnums);

            foreach (var examineeEnum in examineeEnums)
            {
                var match = typeNameMatcher.FindMatchingType(examineeEnum);
                if (match == null)
                {
                    //// TODO [vmaklai] Implement ProcessEnums
                }

                //// TODO [vmaklai] Implement ProcessEnums
            }

            return new EnumComparisonResult();
        }
    }
}