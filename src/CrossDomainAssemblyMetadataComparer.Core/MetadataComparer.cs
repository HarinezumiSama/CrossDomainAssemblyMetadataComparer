using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CrossDomainAssemblyMetadataComparer.Core.Model;
using JetBrains.Annotations;
using Omnifactotum;

namespace CrossDomainAssemblyMetadataComparer.Core
{
    public sealed class MetadataComparer
    {
        private static readonly StringComparer StrictIdentifierComparer = StringComparer.Ordinal;
        private static readonly StringComparer IgnoreCaseIdentifierComparer = StringComparer.OrdinalIgnoreCase;

        public MetadataComparer(
            [NotNull] AssemblyReference examineeAssemblyReference,
            [NotNull] ICollection<AssemblyReference> comparandAssemblyReferences,
            [NotNull] MetadataComparisonParameters parameters)
        {
            if (examineeAssemblyReference == null)
            {
                throw new ArgumentNullException(nameof(examineeAssemblyReference));
            }

            if (comparandAssemblyReferences == null)
            {
                throw new ArgumentNullException(nameof(comparandAssemblyReferences));
            }

            if (comparandAssemblyReferences.Any(item => item == null))
            {
                throw new ArgumentException(
                    @"The collection contains a null element.",
                    nameof(comparandAssemblyReferences));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            ExamineeAssemblyReference = examineeAssemblyReference;
            ComparandAssemblyReferences = comparandAssemblyReferences.ToArray().AsReadOnly();
            Parameters = parameters;
        }

        public MetadataComparer(
            [NotNull] AssemblyReference examineeAssemblyReference,
            [NotNull] AssemblyReference comparandAssemblyReference,
            [NotNull] MetadataComparisonParameters parameters)
            : this(examineeAssemblyReference, comparandAssemblyReference.AsArray(), parameters)
        {
            // Nothing to do
        }

        public MetadataComparer(
            [NotNull] AssemblyReference examineeAssemblyReference,
            [NotNull] ICollection<AssemblyReference> comparandAssemblyReferences)
            : this(examineeAssemblyReference, comparandAssemblyReferences, MetadataComparisonParameters.Default)
        {
            // Nothing to do
        }

        public MetadataComparer(
            [NotNull] AssemblyReference examineeAssemblyReference,
            [NotNull] AssemblyReference comparandAssemblyReference)
            : this(examineeAssemblyReference, comparandAssemblyReference.AsArray())
        {
            // Nothing to do
        }

        [NotNull]
        public AssemblyReference ExamineeAssemblyReference
        {
            get;
        }

        [NotNull]
        public ReadOnlyCollection<AssemblyReference> ComparandAssemblyReferences
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
            var comparandAssemblies = ComparandAssemblyReferences.Select(reference => reference.Load()).ToArray();

            var examineeTypes = examineeAssembly.GetTypes();
            var comparandTypes = comparandAssemblies.SelectMany(assembly => assembly.GetTypes()).ToArray();

            var enumComparisonResult = ProcessEnums(examineeTypes, comparandTypes);

            return new MetadataComparisonResult(enumComparisonResult);
        }

        [NotNull]
        private ICollection<EnumComparisonResult> ProcessEnums(
            [NotNull] ICollection<Type> examineeTypes,
            [NotNull] ICollection<Type> comparandTypes)
        {
            var examineeEnums = examineeTypes.GetEnumTypes();
            var comparandEnums = comparandTypes.GetEnumTypes();

            var typeNameMatcher = Parameters.CreateTypeNameMatcher(comparandEnums);

            var enumComparisonResults = new List<EnumComparisonResult>(examineeEnums.Length);
            foreach (var examineeEnum in examineeEnums)
            {
                var typeMatch = typeNameMatcher.FindMatchingType(examineeEnum);

                ICollection<EnumValueComparisonResult> valueComparisonResults;
                switch (typeMatch.Kind)
                {
                    case TypeMatchKind.Strict:
                    case TypeMatchKind.CaseInsensitive:
                    case TypeMatchKind.UserDefined:
                        var comparandEnum = typeMatch.FoundTypes.Single();
                        valueComparisonResults = ProcessEnumValues(examineeEnum, comparandEnum);
                        break;

                    case TypeMatchKind.Ambiguous:
                    case TypeMatchKind.None:
                        valueComparisonResults = new EnumValueComparisonResult[0];
                        break;

                    default:
                        throw typeMatch.Kind.CreateEnumValueNotImplementedException();
                }

                var enumComparisonResult = new EnumComparisonResult(examineeEnum, typeMatch, valueComparisonResults);
                enumComparisonResults.Add(enumComparisonResult);
            }

            return enumComparisonResults;
        }

        [NotNull]
        private static ICollection<EnumValueComparisonResult> ProcessEnumValues(
            [NotNull] Type examineeEnum,
            [NotNull] Type comparandEnum)
        {
            var examineeValues = examineeEnum.GetOrdinalEnumValues();
            var comparandValues = comparandEnum.GetOrdinalEnumValues();

            var allValues = examineeValues.Concat(comparandValues).Distinct().OrderBy(Factotum.Identity).ToArray();

            var results = allValues
                .Select(
                    value =>
                    {
                        var examineeName = examineeEnum.GetEnumName(value);
                        var comparandName = comparandEnum.GetEnumName(value);
                        return new EnumValueComparisonResult(
                            value,
                            examineeName,
                            comparandName,
                            DetermineEnumValueMatchKind(examineeName, comparandName));
                    })
                .ToArray();

            return results;
        }

        private static EnumValueMatchKind DetermineEnumValueMatchKind(
            [CanBeNull] string examineeName,
            [CanBeNull] string comparandName)
        {
            //// TODO [vmaklai] DetermineEnumValueMatchKind: User defined case

            if (examineeName.IsNullOrEmpty())
            {
                return EnumValueMatchKind.NoExaminee;
            }

            if (comparandName.IsNullOrEmpty())
            {
                return EnumValueMatchKind.NoComparand;
            }

            if (StrictIdentifierComparer.Equals(examineeName, comparandName))
            {
                return EnumValueMatchKind.Strict;
            }

            if (IgnoreCaseIdentifierComparer.Equals(examineeName, comparandName))
            {
                return EnumValueMatchKind.Strict;
            }

            return EnumValueMatchKind.DifferentNames;
        }
    }
}