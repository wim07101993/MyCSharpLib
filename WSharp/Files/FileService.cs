using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Unity;

using WSharp.Dialogs;
using WSharp.Dialogs.Options;
using WSharp.Extensions;
using WSharp.Files.Results;
using WSharp.Logging.Loggers;

namespace WSharp.Files
{
    /// <summary>
    ///     Service to read and write to files. Implements the <see cref="IFileService" />.
    /// </summary>
    public partial class FileService : IFileService
    {
        #region FIELDS

        private readonly IUnityContainer _unityContainer;
        private readonly ILogger _logger;
        private readonly Dictionary<string, CustomReaderWriter> _guessedReaderWriters = new Dictionary<string, CustomReaderWriter>();

        private IDialogService _dialogService;
        private IReaderWriterCollection _readerWriters;

        private IReadOnlyDictionary<Type, IFileReaderWriter> _readonlyReaderWriters;

        #endregion FIELDS

        public FileService(IUnityContainer unityContainer, ILogger logger)
        {
            _unityContainer = unityContainer;
            _logger = logger;
        }

        #region PROPERTIES

        protected IDialogService DialogService => _dialogService ??= _unityContainer.Resolve<IDialogService>();

        protected IReaderWriterCollection InternalReaderWriters
        {
            get => _readerWriters ??= _unityContainer.IsRegistered<IReaderWriterCollection>()
                    ? _unityContainer.Resolve<IReaderWriterCollection>()
                    : new ReaderWriterCollection();
        }

        public IReadOnlyDictionary<Type, IFileReaderWriter> ReaderWriters
            => _readonlyReaderWriters ??= new ReadOnlyDictionary<Type, IFileReaderWriter>(InternalReaderWriters);

        #endregion PROPERTIES

        public IFileService RegisterReaderWriter(Type type, IFileReaderWriter readerWriter)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));
            _ = readerWriter ?? throw new ArgumentNullException(nameof(readerWriter));

            Log($"Registering new ReaderWriter for type {type}");

            if (!type.IsClass)
                throw new ArgumentException($"Type {type} is not a class");
            if (!type.GetInterfaces().Contains(typeof(IFile)))
                throw new ArgumentException($"Type {type} is not a {nameof(IFile)}");

            if (InternalReaderWriters.ContainsKey(type))
                InternalReaderWriters[type] = readerWriter;
            else
                InternalReaderWriters.Add(type, readerWriter);

            Log($"Registered new ReaderWriter for type {type}");
            return this;
        }

        public IFileService RegisterReaderWriter(Type fileType, Type readerWriterType)
        {
            _ = readerWriterType ?? throw new ArgumentNullException(nameof(readerWriterType));

            if (!typeof(IFileReaderWriter).IsAssignableFrom(readerWriterType))
                throw new ArgumentException($"{readerWriterType} is not assignable to a {nameof(IFileReaderWriter)}");

            return RegisterReaderWriter(fileType, (IFileReaderWriter)_unityContainer.Resolve(readerWriterType));
        }

        public ISaveFileResult Save(IFile file, string path, bool forceDialog = false)
        {
            _ = file ?? throw new ArgumentNullException(nameof(file));

            var dialogResult = EDialogResult.None;
            if (string.IsNullOrWhiteSpace(path) || forceDialog)
            {
                var (canceled, newPath) = AskToSaveFile(file, path);
                if (canceled)
                    return new SaveFileResult(EDialogResult.Cancel, file);

                path = newPath;
                dialogResult = EDialogResult.Ok;
            }

            Log($"Writing file to {path}");

            using var stream = File.Create(path);

            if (InternalReaderWriters.TryGetValue(file.GetType(), out var readerWriter))
                readerWriter.Write(file, stream);
            else
                new StreamWriter(stream).SerializeJson(file);

            file.IsSaved = true;
            file.Path = path;

            Log($"Done writing file to {path}");
            return new SaveFileResult(dialogResult, file);
        }

        public async Task<ISaveFileResult> SaveAsync(IFile file, string path, bool forceDialog = false)
        {
            _ = file ?? throw new ArgumentNullException(nameof(file));

            var dialogResult = EDialogResult.None;
            if (string.IsNullOrWhiteSpace(path) || forceDialog)
            {
                var (canceled, newPath) = await AskToSaveFileAsync(file, path);
                if (canceled)
                    return new SaveFileResult(EDialogResult.Cancel, file);

                path = newPath;
                dialogResult = EDialogResult.Ok;
            }

            Log($"Writing file to {path}");

            using var stream = File.Create(path);

            if (InternalReaderWriters.TryGetValue(file.GetType(), out var readerWriter))
                await readerWriter.WriteAsync(file, stream);
            else
                new StreamWriter(stream).SerializeJson(file);

            file.IsSaved = true;
            file.Path = path;

            Log($"Done writing file to {path}");
            return new SaveFileResult(dialogResult, file);
        }

        protected (bool canceled, string newPath) AskToSaveFile(IFile file, string path)
        {
            var type = file.GetType();
            var dialogResult = DialogService.ShowSaveFileDialog(new SaveFileDialogOptions(
                filter: GetFileFilter(type).ToList(),
                addAllFilesFilterOption: true,
                originalFilePath: path,
                valueType: type));

            switch (dialogResult.Result)
            {
                case EDialogResult.Cancel:
                case EDialogResult.Close:
                    return (false, default);

                default:
                    return (true, dialogResult.Path);
            }
        }

        protected async Task<(bool canceled, string newPath)> AskToSaveFileAsync(IFile file, string path)
        {
            var type = file.GetType();
            var dialogResult = await DialogService.ShowSaveFileDialogAsync(new SaveFileDialogOptions(
                filter: GetFileFilter(type).ToList(),
                addAllFilesFilterOption: true,
                originalFilePath: path,
                valueType: type));

            switch (dialogResult.Result)
            {
                case EDialogResult.Cancel:
                case EDialogResult.Close:
                    return (false, default);

                default:
                    return (true, dialogResult.Path);
            }
        }

        #region opening

        public IMultipleOpenFileResult Open(bool canOpenMultiple, IEnumerable<Type> types)
        {
            _ = types ?? throw new ArgumentNullException(nameof(types));
            if (types.Any(x => !typeof(IFile).IsAssignableFrom(x)))
                throw new InvalidOperationException($"Cannot open type that is not assignable to {typeof(IFile).Name}");

            Log($"Opening file of type {types.SerializeJson(Formatting.None)}");
            var (canceled, paths, selectedContentType) = AskToOpenFile(canOpenMultiple, types);
            if (canceled)
            {
                Log($"No files selected");
                return new MultipleOpenFileResult(EDialogResult.Cancel);
            }

            var files = paths.Select(x => Open(selectedContentType, x)).ToArray();

            Log($"Done opening file of type {types.SerializeJson(Formatting.None)}.");
            return new MultipleOpenFileResult(EDialogResult.Ok, files);
        }

        public async Task<IMultipleOpenFileResult> OpenAsync(bool canOpenMultiple, IEnumerable<Type> types)
        {
            _ = types ?? throw new ArgumentNullException(nameof(types));
            if (types.Any(x => !typeof(IFile).IsAssignableFrom(x)))
                throw new InvalidOperationException($"Cannot open type that is not assignable to {typeof(IFile).Name}");

            Log($"Opening file of type {types.SerializeJson(Formatting.None)}");
            var (canceled, paths, selectedContentType) = await AskToOpenFileAsync(canOpenMultiple, types);
            if (canceled)
            {
                Log($"No files selected");
                return new MultipleOpenFileResult(EDialogResult.Cancel);
            }

            var files = paths.Select(x => Open(selectedContentType, x)).ToArray();

            Log($"Done opening file of type {types.SerializeJson(Formatting.None)}.");
            return new MultipleOpenFileResult(EDialogResult.Ok, files);
        }

        protected (bool canceled, string[] paths, Type selectedContentType) AskToOpenFile(
            bool canOpenMultiple, IEnumerable<Type> types)
        {
            var dialogResult = DialogService.ShowOpenFileDialog(new OpenFileDialogOptions(
                filter: GetFileFilter(types).ToList(),
                initialDirectory: Environment.SpecialFolder.MyDocuments.ToString(),
                multiSelect: canOpenMultiple,
                addAllFilesFilterOption: true));

            switch (dialogResult.Result)
            {
                case EDialogResult.Cancel:
                case EDialogResult.Close:
                    return (true, default, default);

                default:
                    return (false, dialogResult.Paths, dialogResult.SelectedFileFilter.FileContentsType);
            }
        }

        protected async Task<(bool canceled, string[] paths, Type selectedContentType)> AskToOpenFileAsync(
            bool canOpenMultiple, IEnumerable<Type> types)
        {
            var dialogResult = await DialogService.ShowOpenFileDialogAsync(new OpenFileDialogOptions(
                filter: GetFileFilter(types).ToList(),
                initialDirectory: Environment.SpecialFolder.MyDocuments.ToString(),
                multiSelect: canOpenMultiple,
                addAllFilesFilterOption: true));

            switch (dialogResult.Result)
            {
                case EDialogResult.Cancel:
                case EDialogResult.Close:
                    return (true, default, default);

                default:
                    return (false, dialogResult.Paths, dialogResult.SelectedFileFilter.FileContentsType);
            }
        }

        public IFile Open(Type type, string filePath)
        {
            _ = filePath ?? throw new ArgumentNullException(nameof(filePath));
            if (type != null && !typeof(IFile).IsAssignableFrom(type))
                throw new InvalidOperationException($"Cannot open type that is not assignable to {typeof(IFile).Name}");

            if (type != null && InternalReaderWriters.TryGetValue(type, out var internalReaderWriter))
            {
                Log($"Using reader {internalReaderWriter}");
                var file = TryReadFile(internalReaderWriter.Read, filePath);
                if (file != null)
                    return file;
            }
            else
            {
                Log("No reader detected, started guessing");
                foreach (var readerWriter in GetReadersToTry(type, filePath))
                {
                    var file = TryReadFile(readerWriter.Read, filePath);
                    if (file != null)
                    {
                        Log("Found correct reader");
                        _guessedReaderWriters.Add(filePath, readerWriter);
                        return file;
                    }
                }
            }

            throw new Exception("Could not read the given file.");

            static IFile TryReadFile(Func<Stream, IFile> reader, string filePath)
            {
                try
                {
                    using var stream = File.OpenRead(filePath);

                    var file = reader(stream);
                    if (file == null)
                        return null;

                    file.Path = filePath;
                    file.IsSaved = true;
                    return file;
                }
                catch
                {
                    // IGNORED
                    return null;
                }
            }
        }

        public async Task<IFile> OpenAsync(Type type, string filePath)
        {
            _ = filePath ?? throw new ArgumentNullException(nameof(filePath));
            if (type != null && !typeof(IFile).IsAssignableFrom(type))
                throw new InvalidOperationException($"Cannot open type that is not assignable to {typeof(IFile).Name}");

            if (type != null && InternalReaderWriters.TryGetValue(type, out var internalReaderWriter))
            {
                Log($"Using reader {internalReaderWriter}");
                var file = await TryReadFileAsync(internalReaderWriter.ReadAsync, filePath);
                if (file != null)
                    return file;
            }
            else
            {
                Log("No reader detected, started guessing");
                foreach (var readerWriter in GetReadersToTry(type, filePath))
                {
                    var file = await TryReadFileAsync(readerWriter.ReadAsync, filePath);
                    if (file != null)
                    {
                        Log("Found correct reader");
                        _guessedReaderWriters.Add(filePath, readerWriter);
                        return file;
                    }
                }
            }

            throw new Exception("Could not read the given file.");

            static async Task<IFile> TryReadFileAsync(Func<Stream, Task<IFile>> reader, string filePath)
            {
                try
                {
                    using var stream = File.OpenRead(filePath);

                    var file = await reader(stream);
                    if (file == null)
                        return null;

                    file.Path = filePath;
                    file.IsSaved = true;
                    return file;
                }
                catch
                {
                    // IGNORED
                    return null;
                }
            }
        }

        private IEnumerable<CustomReaderWriter> GetReadersToTry(Type type, string filePath)
        {
            var extension = Path.GetExtension(filePath);

            if (_guessedReaderWriters.Any())
            {
                Log("Trying previous used custom readers/writers");
                if (_guessedReaderWriters.TryGetValue(filePath, out var previouslyUsedValue))
                    yield return previouslyUsedValue;

                var customSerializers = _guessedReaderWriters
                    .Where(x => x.Key != filePath)
                    .ToList();

                var customSerializersWithCorrectExtension = _guessedReaderWriters.Where(x => Path.GetExtension(x.Key) == extension);
                foreach (var customSerializer in customSerializersWithCorrectExtension)
                {
                    _ = customSerializers.Remove(customSerializer);
                    yield return customSerializer.Value;
                }

                foreach (var customSerializer in customSerializers)
                    yield return customSerializer.Value;
            }

            var registeredReaderWriters = ReaderWriters.Values.ToList();

            Log("Trying registered readers with correct file extension");
            var readerWritersWithCorrectExtension = ReaderWriters.Values
                .Where(rw => rw.FileFilter.Any(filter => filter.Extensions.Substring(1) == extension));

            foreach (var readerWriter in readerWritersWithCorrectExtension)
            {
                _ = registeredReaderWriters.Remove(readerWriter);
                yield return new CustomReaderWriter(readerWriter.Read);
            }

            switch (extension)
            {
                case "json":
                case "js":
                    yield return new CustomReaderWriter(EReaderType.Json, type);
                    break;

                case "xml":
                    yield return new CustomReaderWriter(EReaderType.Xml, type);
                    break;

            }

            if (extension == ".json")


                Log("Trying registered readers with incorrect file extension");
            foreach (var readerWriter in registeredReaderWriters)
                yield return new CustomReaderWriter(
                    readerWriter.Read, readerWriter.ReadAsync,
                    readerWriter.Write, readerWriter.WriteAsync);
        }

        #endregion opening

        #region close

        public IMultipleCloseFileResult Close(IList<IFile> files)
        {
            Log("Closing files");

            _ = files ?? throw new ArgumentNullException(nameof(files));
            if (files.Count == 0)
                return new MultipleCloseFileResult(EDialogResult.None, false);

            var filesToSave = new List<IFile>();
            var alreadySavedFiles = new List<IFile>();
            foreach (var file in files)
            {
                if (file.IsSaved)
                    alreadySavedFiles.Add(file);
                else
                    filesToSave.Add(file);
            }

            var dialogResult = EDialogResult.None;
            var savedFiles = false;
            if (filesToSave.Any())
            {
                var (canceled, filesToActuallySave) = AskToSaveUnSavedFiles(filesToSave);
                if (canceled)
                    return new MultipleCloseFileResult(EDialogResult.Cancel, false);

                var results = this.Save(filesToActuallySave);
                savedFiles = results.Any(x => (x.DialogResult & (EDialogResult.None | EDialogResult.Ok)) > 0);
                if (results.Any(x => x.DialogResult == EDialogResult.Cancel))
                    return new MultipleCloseFileResult(EDialogResult.Cancel, savedFiles);

                dialogResult = EDialogResult.Ok;
            }

            foreach (var file in files)
                file.Dispose();

            return new MultipleCloseFileResult(dialogResult, savedFiles);
        }

        public async Task<IMultipleCloseFileResult> CloseAsync(IList<IFile> files)
        {
            Log("Closing files");

            _ = files ?? throw new ArgumentNullException(nameof(files));
            if (files.Count == 0)
                return new MultipleCloseFileResult(EDialogResult.None, false);

            var filesToSave = new List<IFile>();
            var alreadySavedFiles = new List<IFile>();
            foreach (var file in files)
            {
                if (file.IsSaved)
                    alreadySavedFiles.Add(file);
                else
                    filesToSave.Add(file);
            }

            var dialogResult = EDialogResult.None;
            var savedFiles = false;
            if (filesToSave.Any())
            {
                var (canceled, filesToActuallySave) = await AskToSaveUnSavedFilesAsync(filesToSave);
                if (canceled)
                    return new MultipleCloseFileResult(EDialogResult.Cancel, false);

                var results = this.Save(filesToActuallySave);
                savedFiles = results.Any(x => (x.DialogResult & (EDialogResult.None | EDialogResult.Ok)) > 0);
                if (results.Any(x => x.DialogResult == EDialogResult.Cancel))
                    return new MultipleCloseFileResult(EDialogResult.Cancel, savedFiles);

                dialogResult = EDialogResult.Ok;
            }

            foreach (var file in files)
                file.Dispose();

            return new MultipleCloseFileResult(dialogResult, savedFiles);
        }

        protected (bool canceled, IEnumerable<IFile> filesToSave) AskToSaveUnSavedFiles(IReadOnlyCollection<IFile> files)
        {
            var dialogResult = DialogService.ShowSaveUnsavedFilesDialog(new SaveUnsavedFilesDialogOptions(files));

            switch (dialogResult.Result)
            {
                case EDialogResult.Cancel:
                case EDialogResult.Close:
                    return (true, null);

                default:
                    return (false, dialogResult.FilesToSave
                        .Where(x => x.shouldSave)
                        .Select(x => x.file)
                        .ToList());
            }
        }

        protected async Task<(bool canceled, IEnumerable<IFile> filesToSave)> AskToSaveUnSavedFilesAsync(IReadOnlyCollection<IFile> files)
        {
            var dialogResult = await DialogService.ShowSaveUnsavedFilesDialogAsync(new SaveUnsavedFilesDialogOptions(files));

            switch (dialogResult.Result)
            {
                case EDialogResult.Cancel:
                case EDialogResult.Close:
                    return (true, null);

                default:
                    return (false, dialogResult.FilesToSave
                        .Where(x => x.shouldSave)
                        .Select(x => x.file)
                        .ToList());
            }
        }

        #endregion close

        protected IEnumerable<FileFilter> GetFileFilter(params Type[] types)
            => GetFileFilter((IEnumerable<Type>)types);

        protected IEnumerable<FileFilter> GetFileFilter(IEnumerable<Type> types)
        {
            return types
                .Select(x =>
                {
                    var success = InternalReaderWriters.TryGetValue(x, out var readerWriter);
                    return (success, readerWriter);
                })
                .Where(x => x.success)
                .Select(x => x.readerWriter.FileFilter)
                .SelectMany(x => x)
                .Distinct();
        }

        protected void Log(object o, TraceEventType eventType = TraceEventType.Verbose, [CallerMemberName] string tag = null)
            => _logger?.LogAsync(GetType().Name, o, eventType, tag);

        protected void Log<T>(string title, T payload, TraceEventType eventType = TraceEventType.Verbose, [CallerMemberName] string tag = null)
            => _logger.LogAsync(eventType, GetType().Name, tag, title, new object[] { payload });
    }
}
