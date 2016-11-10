using System;
using System.Collections.Generic;
using System.Linq;
using CrossDomainAssemblyMetadataComparer.Core.Model;
using JetBrains.Annotations;
using Omnifactotum;

namespace CrossDomainAssemblyMetadataComparer.Core
{
    internal static class InternalHelper
    {
        [NotNull]
        public static object[] GetOrdinalEnumValues(this Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException(nameof(enumType));
            }

            if (!enumType.IsEnum)
            {
                throw new ArgumentException($"The type '{enumType}' is not an enumeration.", nameof(enumType));
            }

            var values = Enum.GetValues(enumType);
            var underlyingType = Enum.GetUnderlyingType(enumType);

            var result = new object[values.Length];
            for (var index = 0; index < values.Length; index++)
            {
                result[index] = Convert.ChangeType(values.GetValue(index), underlyingType);
            }

            return result;
        }

        [CanBeNull]
        public static string GetEnumName([NotNull] this Type enumType, [NotNull] object value)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException(nameof(enumType));
            }

            if (!enumType.IsEnum)
            {
                throw new ArgumentException($"The type '{enumType}' is not an enumeration.", nameof(enumType));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return Enum.IsDefined(enumType, value) ? Enum.GetName(enumType, value) : null;
        }

        [NotNull]
        public static Type[] GetEnumTypes([NotNull] this ICollection<Type> types)
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            if (types.Any(item => item == null))
            {
                throw new ArgumentException(@"The collection contains a null element.", nameof(types));
            }

            return types.Where(t => t.IsEnum).ToArray();
        }

        public static OverallMatchKind ToOverallMatchKind(this EnumValueMatchKind enumValueMatchKind)
        {
            switch (enumValueMatchKind)
            {
                case EnumValueMatchKind.Strict:
                    return OverallMatchKind.Strict;

                case EnumValueMatchKind.CaseInsensitive:
                    return OverallMatchKind.CaseInsensitive;

                case EnumValueMatchKind.UserDefined:
                    return OverallMatchKind.UserDefined;

                case EnumValueMatchKind.DifferentNames:
                case EnumValueMatchKind.NoExaminee:
                case EnumValueMatchKind.NoComparand:
                    return OverallMatchKind.MismatchEncountered;

                default:
                    throw enumValueMatchKind.CreateEnumValueNotImplementedException();
            }
        }

        public static OverallMatchKind ToOverallMatchKind(this TypeMatchKind typeMatchKind)
        {
            switch (typeMatchKind)
            {
                case TypeMatchKind.Strict:
                    return OverallMatchKind.Strict;

                case TypeMatchKind.CaseInsensitive:
                    return OverallMatchKind.CaseInsensitive;

                case TypeMatchKind.UserDefined:
                    return OverallMatchKind.UserDefined;

                case TypeMatchKind.None:
                case TypeMatchKind.Ambiguous:
                    return OverallMatchKind.MismatchEncountered;

                default:
                    throw typeMatchKind.CreateEnumValueNotImplementedException();
            }
        }

        public static OverallMatchKind? ComputeOverallMatchKind(
            [NotNull] this ICollection<OverallMatchKind> innerOverallMatchKinds)
        {
            if (innerOverallMatchKinds == null)
            {
                throw new ArgumentNullException(nameof(innerOverallMatchKinds));
            }

            return innerOverallMatchKinds.Count == 0 ? (OverallMatchKind?)null : innerOverallMatchKinds.Max();
        }

        public static OverallMatchKind ComputeOverallMatchKind(
            this OverallMatchKind baseOverallMatchKind,
            [NotNull] ICollection<OverallMatchKind> innerOverallMatchKinds)
        {
            if (innerOverallMatchKinds == null)
            {
                throw new ArgumentNullException(nameof(innerOverallMatchKinds));
            }

            var result = baseOverallMatchKind;

            var computedInnerOverallMatchKind = innerOverallMatchKinds.ComputeOverallMatchKind();
            if (computedInnerOverallMatchKind.HasValue)
            {
                result = Factotum.Max(result, computedInnerOverallMatchKind.Value);
            }

            return result;
        }
    }
}