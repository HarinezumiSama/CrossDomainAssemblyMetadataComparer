namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public enum EnumValueMatchKind
    {
        /// <summary>
        ///     There are named values in the both examinee and comparand enumerations that correspond
        ///     to the specified ordinal value, however these names do not match according to any
        ///     rules (both default and user defined).
        /// </summary>
        DifferentNames,

        /// <summary>
        ///     There is no named value in the examinee enumeration that corresponds to the specified
        ///     ordinal value.
        /// </summary>
        NoExaminee,

        /// <summary>
        ///     There is no named value in the comparand enumeration that corresponds to the
        ///     specified ordinal value.
        /// </summary>
        NoComparand,

        /// <summary>
        ///     There is a named enumeration value that corresponds to the specified ordinal value
        ///     and its name is exactly the same.
        /// </summary>
        Strict,

        /// <summary>
        ///     There is a named enumeration value that corresponds to the specified ordinal value
        ///     and its name is the same if compared case-insensitive.
        /// </summary>
        CaseInsensitive,

        /// <summary>
        ///     There is a named enumeration value that corresponds to the specified ordinal value
        ///     and its name is considered a match according to the rules defined by a user.
        /// </summary>
        UserDefined
    }
}