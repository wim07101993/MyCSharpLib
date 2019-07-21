using MyCSharpLib.Services.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MyCSharpLib.Services
{
    /// <summary>
    /// Service to read and write to files.
    /// Implements the <see cref="IFileService"/>.
    /// </summary>
    public class FileService : IFileService
    {
        #region FIELDS

        private readonly ISettingsProvider _settingsProvider;
        private readonly ISerializer _serializer;
        private readonly IDeserializer _deserializer;
        private readonly ICryptoTransform _cryptoTransform;

        #endregion FIELDS


        #region CONSTRUCTOR

        public FileService(ISettingsProvider settingsProvider, ISerializer serializer, IDeserializer deserializer, ICryptoTransform cryptoTransform)
        {
            _settingsProvider = settingsProvider;
            _serializer = serializer;
            _deserializer = deserializer;
            _cryptoTransform = cryptoTransform;
        }

        #endregion CONSTRUCTOR


        #region PROPERTIES

        public string LogDirectory => throw new NotImplementedException();

        #endregion PROPERTIES


        #region METHODS

        public string GenerateDataFilePath<T>(string extension)
        {
            var type = typeof(T);
            return $@"{_settingsProvider.Settings.FileSettings.DataDirectory}\{type.Name}.{extension}";
        }

        private string GetPath<T>(string path, string extension)
        {
            var type = typeof(T);
            if (string.IsNullOrWhiteSpace(path))
                path = _settingsProvider.Settings.FileSettings.FilePaths.FirstOrDefault(x => x.Key == type).Value;

            if (!string.IsNullOrWhiteSpace(path))
                return path;

            path = GenerateDataFilePath<T>(extension);
            _settingsProvider.Settings.FileSettings.FilePaths.Add(typeof(T), path);
            return path;
        }

        #region read

        /// <summary>
        /// Read the text from a specified path asynchronously
        /// </summary>
        /// <param name="path">Path to the file to read the content from</param>
        /// <returns>The string content of the file</returns>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required permission.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Path is a zero-length string, contains only white space, or contains one or more 
        ///     invalid characters as defined by <see cref="Path.InvalidPathChars"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Path is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The number of characters is larger than <see cref="int.MaxValue"/>.
        /// </exception>
        /// <exception cref="PathTooLongException">
        ///     The specified path, file name, or both exceed the system-defined maximum length.
        ///     For example, on Windows-based platforms, paths must be less than 248 characters,
        ///     and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     The specified path is invalid, (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="FileNotFoundException">
        ///     The file specified in path was not found.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     Path is in an invalid format.
        /// </exception>
        public async Task<string> ReadTextAsync(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            using (var fileStream = File.OpenText(path))
                return await fileStream.ReadToEndAsync();
        }

        public async Task<string> ReadEncryptedTextAsync(string path, ICryptoTransform cryptoTransform = null)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (cryptoTransform == null)
                cryptoTransform = _cryptoTransform;

            using (var fileStream = File.OpenRead(path))
            using (var cryptoStream = new CryptoStream(fileStream, cryptoTransform, CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(cryptoStream))
                return await streamReader.ReadToEndAsync();
        }

        /// <summary>
        /// Reads all lines from a specified file.
        /// </summary>
        /// <param name="path">Path to the file to read from.</param>
        /// <returns>The lines of the file.</returns>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required permission.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Path is a zero-length string, contains only white space, or contains one or more 
        ///     invalid characters as defined by <see cref="Path.InvalidPathChars"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Path is null.
        /// </exception>
        /// <exception cref="PathTooLongException">
        ///     The specified path, file name, or both exceed the system-defined maximum length.
        ///     For example, on Windows-based platforms, paths must be less than 248 characters,
        ///     and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     The specified path is invalid, (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="FileNotFoundException">
        ///     The file specified in path was not found.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     Path is in an invalid format.
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        ///     There is insufficient memory to allocate a buffer for the returned string.
        /// </exception>
        /// <exception cref="IOException">
        ///     An I/O error occurs.
        /// </exception>
        public IEnumerable<string> ReadLines(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            using (var fileStream = File.OpenText(path))
            {
                var line = fileStream.ReadLine();
                while (line != null)
                {
                    yield return line;
                    line = fileStream.ReadLine();
                }
            }
        }

        public IEnumerable<string> ReadEncryptedLines(string path, ICryptoTransform cryptoTransform = null)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (cryptoTransform == null)
                cryptoTransform = _cryptoTransform;

            using (var fileStream = File.OpenRead(path))
            using (var cryptoStream = new CryptoStream(fileStream, cryptoTransform, CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(cryptoStream))
            {
                var line = streamReader.ReadLine();
                while (line != null)
                {
                    yield return line;
                    line = streamReader.ReadLine();
                }
            }
        }

        public async Task<T> ReadAsync<T>(string path = null, IDeserializer deserializer = null)
        {
            if (path == null)
            {
                try
                {
                    path = _settingsProvider.Settings.FileSettings.FilePaths[typeof(T)];
                }
                catch (KeyNotFoundException e)
                {
                    throw new KeyNotFoundException($"There was no path found for type {typeof(T).Name}", e);
                }
            }

            if (deserializer == null)
                deserializer = _deserializer;

            using (var fileStream = File.OpenText(path))
                return await deserializer.DeserializeAsync<T>(fileStream);
        }

        public async Task<T> ReadEncryptedAsync<T>(string path = null, IDeserializer deserializer = null, ICryptoTransform cryptoTransform = null)
        {
            if (path == null)
            {
                try
                {
                    path = _settingsProvider.Settings.FileSettings.FilePaths[typeof(T)];
                }
                catch (KeyNotFoundException e)
                {
                    throw new KeyNotFoundException($"There was no path found for type {typeof(T).Name}", e);
                }
            }

            if (deserializer == null)
                deserializer = _deserializer;

            if (cryptoTransform == null)
                cryptoTransform = _cryptoTransform;

            using (var fileStream = File.OpenRead(path))
            using (var cryptoStream = new CryptoStream(fileStream, cryptoTransform, CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(cryptoStream))
                return await deserializer.DeserializeAsync<T>(streamReader);
        }

        #endregion read


        #region write

        /// <summary>
        /// Writes a string to a file asynchronously.
        /// </summary>
        /// <param name="text">Text to write to the file.</param>
        /// <param name="path">Path to the file to write to.</param>
        /// <exception cref="IOException">
        ///     The directory specified by path is a file.-or-The network name is not known. 
        ///     Or An I/O error occurred while opening the file.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required permission.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     path is a zero-length string, contains only white space, or contains one or more
        ///     invalid characters. You can query for invalid characters by using the <see cref="Path.GetInvalidPathChars"/>
        ///     method.-or- path is prefixed with, or contains, only a colon character (:).
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     path is null.
        /// </exception>
        /// <exception cref="PathTooLongException">
        ///     The specified path, file name, or both exceed the system-defined maximum length.
        ///     For example, on Windows-based platforms, paths must be less than 248 characters
        ///     and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     The specified path is invalid (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     path is in an invalid format.
        /// </exception>
        public async Task WriteTextAsync(string text, string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (string.IsNullOrEmpty(text))
            {
                File.Create(path);
                return;
            }

            using (var fileStream = File.Open(path, FileMode.Create))
            using (var streamWriter = new StreamWriter(fileStream))
            {
                await streamWriter.WriteAsync(text);
                await streamWriter.FlushAsync();
            }
        }

        public async Task WriteEncryptedTextAsync(string text, string path, ICryptoTransform cryptoTransform = null)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (string.IsNullOrEmpty(text))
            {
                File.Create(path);
                return;
            }

            if (cryptoTransform == null)
                cryptoTransform = _cryptoTransform;

            using (var fileStream = File.Open(path, FileMode.Create))
            using (var cryptoStream = new CryptoStream(fileStream, cryptoTransform, CryptoStreamMode.Write))
            using (var streamWriter = new StreamWriter(cryptoStream))
            {
                await streamWriter.WriteAsync(text);
                await streamWriter.FlushAsync();
            }
        }

        /// <summary>
        /// Writes a list of lines to a file asynchronoulsy.
        /// </summary>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="text">Lines to write to the file.</param>
        /// <exception cref="IOException">
        ///     The directory specified by path is a file.-or-The network name is not known. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required permission.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     path is a zero-length string, contains only white space, or contains one or more
        ///     invalid characters. You can query for invalid characters by using the <see cref="Path.GetInvalidPathChars"/>
        ///     method.-or- path is prefixed with, or contains, only a colon character (:).
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     path is null.
        /// </exception>
        /// <exception cref="PathTooLongException">
        ///     The specified path, file name, or both exceed the system-defined maximum length.
        ///     For example, on Windows-based platforms, paths must be less than 248 characters
        ///     and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     The specified path is invalid (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     path is in an invalid format.
        /// </exception>
        public async Task WriteLinesAsync(IEnumerable<string> lines, string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (lines == null)
                throw new ArgumentNullException(nameof(lines));

            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var fileStream = File.Open(path, FileMode.Create))
            using (var streamWriter = new StreamWriter(fileStream))
            {
                foreach (var line in lines)
                {
                    await streamWriter.WriteLineAsync(line);
                    await streamWriter.FlushAsync();
                }
            }
        }

        public async Task WriteEncryptedLinesAsync(IEnumerable<string> lines, string path, ICryptoTransform cryptoTransform = null)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (lines == null)
                throw new ArgumentNullException(nameof(lines));

            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (cryptoTransform == null)
                cryptoTransform = _cryptoTransform;

            using (var fileStream = File.OpenRead(path))
            using (var cryptoStream = new CryptoStream(fileStream, cryptoTransform, CryptoStreamMode.Write))
            using (var streamWriter = new StreamWriter(cryptoStream))
            {
                foreach (var line in lines)
                {
                    await streamWriter.WriteLineAsync(line);
                    await streamWriter.FlushAsync();
                }
            }
        }

        public async Task WriteAsync<T>(T value, string path = null, ISerializer serializer = null)
        {
            if (serializer == null)
                serializer = _serializer;

            path = GetPath<T>(path, serializer.FileExtension);

            using (var fileStream = File.Open(path, FileMode.Create))
            using (var streamWriter = new StreamWriter(fileStream))
                await serializer.SerializeAsync(value, streamWriter);
        }

        public async Task WriteEncryptedAsync<T>(T value, string path = null, ISerializer serializer = null, ICryptoTransform cryptoTransform = null)
        {
            if (serializer == null)
                serializer = _serializer;

            path = GetPath<T>(path, serializer.FileExtension);

            using (var fileStream = File.Open(path, FileMode.Create))
            using (var cryptoStream = new CryptoStream(fileStream, cryptoTransform, CryptoStreamMode.Write))
            using (var streamWriter = new StreamWriter(cryptoStream))
                await serializer.SerializeAsync(value, streamWriter);
        }

        #endregion write

        #endregion METHODS
    }
}
