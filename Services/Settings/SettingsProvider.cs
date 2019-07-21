using System.IO;
using System.Threading.Tasks;

namespace MyCSharpLib.Services
{
    /// <summary>
    /// Provider to get <see cref="Settings"/> from the settings file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SettingsProvider<T> : ASettingsProvider<T> where T : class, ISettings, new()
    {
        #region FIELDS

        /// <summary>Service used to fetch and save the settings.</summary>
        private readonly IFileService _fileService;

        #endregion FIELDS


        #region CONSTRUCTOR

        /// <summary>
        /// Constructs a new instance of the <see cref="SettingsProvider{T}"/> class.
        /// </summary>
        /// <param name="fileService">Service to do the saving and fetching of the settings file.</param>
        public SettingsProvider(IFileService fileService)
        {
            _fileService = fileService;
        }

        #endregion CONSTRUCTOR

        #region METHODS

        /// <summary>
        /// Internal method to perform the actual fetching of the settings.
        /// </summary>
        /// <returns>The setting for the application.</returns>
        protected override async Task<T> InternalFetchSettingsAsync()
        {
            var directory = Path.GetDirectoryName(FileServiceSettings.SettingsPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return await _fileService.ReadAsync<T>(FileServiceSettings.SettingsPath);
        }

        /// <summary>
        /// Internal method to perform the actual saving of the settings.
        /// </summary>
        /// <param name="settings">The settings to save.</param>
        protected override async Task InternalSaveSettingsAsync(T settings)
        {
            var directory = Path.GetDirectoryName(FileServiceSettings.SettingsPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            await _fileService.WriteAsync(settings, FileServiceSettings.SettingsPath);
        }

        #endregion METHODS
    }
}
