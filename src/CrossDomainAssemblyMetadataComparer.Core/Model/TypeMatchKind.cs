namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public enum TypeMatchKind
    {
        /// <summary>
        ///     No matching type was found.
        /// </summary>
        None,

        /// <summary>
        ///     A single type with exactly the same name was found.
        /// </summary>
        Strict,

        /// <summary>
        ///     A single type was found which name is the same if compared case-insensitive.
        /// </summary>
        CaseInsensitive,

        /// <summary>
        ///     A single type was found according to the search rules defined by a user.
        /// </summary>
        UserDefined,

        /// <summary>
        ///     Two or more matching types were found.
        /// </summary>
        Ambiguous
    }
}