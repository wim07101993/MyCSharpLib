using System.IO;
using System.Threading.Tasks;

namespace MyCSharpLib.Services
{
    public class SettingsProvider<T> : ASettingsProvider<T> where T : Settings, new()
    {
        #region FIELDS

        private readonly IFileService _fileService;

        #endregion FIELDS


        #region CONSTRUCTOR

        public SettingsProvider(IFileService fileService)
        {
            _fileService = fileService;
        }

        #endregion CONSTRUCTOR

        #region METHODS

        protected override async Task<T> InternalFetchSettingsAsync()
        {
            var directory = Path.GetDirectoryName(FileServiceSettings.SettingsPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return await _fileService.ReadAsync<T>(FileServiceSettings.SettingsPath);
        }

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
