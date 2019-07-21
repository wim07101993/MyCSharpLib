namespace MyCSharpLib.Services
{
    /// <summary>
    /// Settings interface specifically for the <see cref="FileService"/>.
    /// </summary>
    public interface ISettingsForFileService : ISettings
    {
        /// <summary>
        /// Settings to make the <see cref="FileService"/> work
        /// </summary>
        FileServiceSettings FileSettings { get; set; }
    }
}
