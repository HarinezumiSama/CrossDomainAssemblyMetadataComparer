using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core
{
    internal static class InternalHelper
    {
        [NotNull]
        public static object[] GetOrdinalEnumValues(Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException(nameof(enumType));
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
        public static string GetEnumName([NotNull] Type enumType, [NotNull] object value)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException(nameof(enumType));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return Enum.IsDefined(enumType, value) ? Enum.GetName(enumType, value) : null;
        }

        [NotNull]
        public static Type[] GetEnumTypes([NotNull] this ICollection<Type> examineeTypes)
        {
            if (examineeTypes == null)
            {
                throw new ArgumentNullException(nameof(examineeTypes));
            }

            return examineeTypes.Where(t => t.IsEnum).ToArray();
        }
    }
}