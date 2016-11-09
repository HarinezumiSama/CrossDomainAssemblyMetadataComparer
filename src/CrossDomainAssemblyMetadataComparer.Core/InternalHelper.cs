using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

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
    }
}