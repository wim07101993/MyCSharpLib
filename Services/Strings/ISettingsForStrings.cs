namespace MyCSharpLib.Services.Strings
{
    /// <summary>
    /// Settings interface specifically for the <see cref="StringsProvider{T}"/>.
    /// </summary>
    public interface ISettingsForStrings
    {
        /// <summary>
        /// Settings to make the <see cref="StringsProvider{T}"/> work.
        /// </summary>
        StringsSettings StringsSettings { get; set; }
    }
}
