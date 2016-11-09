namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    /// <summary>
    ///     Specifies the overall match kind for a comparison result, including its inner comparison results.
    /// </summary>
    /// <remarks>
    ///     The values of this enumeration are ordered by priority: the maximum value wins.
    /// </remarks>
    public enum OverallMatchKind
    {
        Strict = 0,
        CaseInsensitive = 1,
        UserDefined = 2,
        MismatchEncountered = 3
    }
}