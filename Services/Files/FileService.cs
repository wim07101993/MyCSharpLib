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

        #endregion FIELDS


        #region CONSTRUCTOR

        public FileService(ISettingsProvider settingsProvider, ISerializer serializer, IDeserializer deserializer, ICryptoTransform cryptoTransform)
        {
            _settingsProvider = settingsProvider;
            Serializer = serializer;
            Deserializer = deserializer;
            CryptoTransform = cryptoTransform;
        }

        #endregion CONSTRUCTOR


        #region PROPERTIES

        /// <summary>Serializer used to serialize objects to write to a file.</summary>
        public ISerializer Serializer { get; }

        /// <summary>Deserializer used to deserialize objects read from a file.</summary>
        public IDeserializer Deserializer { get; }

        /// <summary>Transformer used to encrypt and decrypt data to write and read from encrypted files.</summary>
        public ICryptoTransform CryptoTransform { get; }

        #endregion PROPERTIES


        #region METHODS

        /// <summary>
        /// Generates a new data file path for type T.
        /// {data directory}\{type name}.{extension}
        /// </summary>
        /// <typeparam name="T">Type for which the file is ment.</typeparam>
        /// <param name="extension">The extension of the file</param>
        /// <returns>The path to write a file to.</returns>
        public string GenerateDataFilePath<T>(string extension)
        {
            var type = typeof(T);
            return $@"{_settingsProvider.Settings.FileSettings.DataDirectory}\{type.Name}.{extension}";
        }

        /// <summary>
        /// If the path is not null or empty, it is just returned.
        /// Else the path is searched for in the dictionary in the settings.
        /// Finally if there is no path yet for the type, it is generated and added.
        /// </summary>
        /// <typeparam name="T">Type of the data to store in the file.</typeparam>
        /// <param name="path">
        /// Path to save the file to. If this parameter is null, it is filled in with the correct value 
        /// (generated or from the dictionary)
        /// </param>
        /// <param name="extension"></param>
        /// <returns></returns>
        private string GetPath<T>(string extension, string path = null)
        {
            if (!string.IsNullOrWhiteSpace(path))
                return path;

            var type = typeof(T);
            path = _settingsProvider.Settings.FileSettings.FilePaths.FirstOrDefault(x => x.Key == type).Value;

            if (!string.IsNullOrWhiteSpace(path))
                return path;

            path = GenerateDataFilePath<T>(extension);
            _settingsProvider.Settings.FileSettings.FilePaths.Add(typeof(T), path);
            return path;
        }

        #region read

        /// <summary>
        /// Read the text from a specified path as text.
        /// </summary>
        /// <param name="path">Path to the file to read the content from.</param>
        /// <returns>The string content of the file.</returns>
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

        /// <summary>
        /// Read the encrypted text from a specified encrypted path.
        /// </summary>
        /// <param name="path">Path to the file to read the content from.</param>
        /// <param name="cryptoTransform">Transformer that is use to decrypt the data. If this parameter is null, the property value is used.</param>
        /// <returns>The string content of the file.</returns>
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
        public async Task<string> ReadEncryptedTextAsync(string path, ICryptoTransform cryptoTransform = null)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (cryptoTransform == null)
                cryptoTransform = CryptoTransform;

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

        /// <summary>
        /// Reads all lines from a specified encrypted file.
        /// </summary>
        /// <param name="path">Path to the file to read from.</param>
        /// <param name="cryptoTransform">Transformer that is use to decrypt the data. If this parameter is null, the property value is used.</param>
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
        public IEnumerable<string> ReadEncryptedLines(string path, ICryptoTransform cryptoTransform = null)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (cryptoTransform == null)
                cryptoTransform = CryptoTransform;

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

        /// <summary>
        /// Reads the content of a file and parses it to an object.
        /// TODO exceptions
        /// </summary>
        /// <typeparam name="T">Type to parse the content to.</typeparam>
        /// <param name="path">Path to the file to read the json from.</param>
        /// <param name="deserializer">Deserializer to deserialize the content of the file. If this parameter is null, the property value is used.</param>
        /// <returns>The parsed value.</returns>
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
                deserializer = Deserializer;

            using (var fileStream = File.OpenText(path))
                return await deserializer.DeserializeAsync<T>(fileStream);
        }

        /// <summary>
        /// Reads the encrypted content of a file and parses it to an object.
        /// TODO exceptions
        /// </summary>
        /// <typeparam name="T">Type to parse the content to.</typeparam>
        /// <param name="path">Path to the file to read the json from.</param>
        /// <param name="deserializer">Deserializer to deserialize the content of the file. If this parameter is null, the property value is used.</param>
        /// <param name="cryptoTransform">Transformer that is use to decrypt the data. If this parameter is null, the property value is used.</param>
        /// <returns>The parsed value.</returns>
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
                deserializer = Deserializer;

            if (cryptoTransform == null)
                cryptoTransform = CryptoTransform;

            using (var fileStream = File.OpenRead(path))
            using (var cryptoStream = new CryptoStream(fileStream, cryptoTransform, CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(cryptoStream))
                return await deserializer.DeserializeAsync<T>(streamReader);
        }

        #endregion read


        #region write

        /// <summary>
        /// Writes a string to a file.
        /// TODO exceptions
        /// </summary>
        /// <param name="text">Text to write to the file.</param>
        /// <param name="path">Path to the file to write to.</param>
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

        /// <summary>
        /// Writes a string encrypted to a file.
        /// TODO exceptions
        /// </summary>
        /// <param name="text">Text to write to the file.</param>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="cryptoTransform">Transformer that is use to encrypt the data. If this parameter is null, the property value is used.</param>
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
                cryptoTransform = CryptoTransform;

            using (var fileStream = File.Open(path, FileMode.Create))
            using (var cryptoStream = new CryptoStream(fileStream, cryptoTransform, CryptoStreamMode.Write))
            using (var streamWriter = new StreamWriter(cryptoStream))
            {
                await streamWriter.WriteAsync(text);
                await streamWriter.FlushAsync();
            }
        }

        /// <summary>
        /// Writes a list of lines to a file.
        /// TODO exceptions
        /// </summary>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="text">Lines to write to the file.</param>
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

        /// <summary>
        /// Writes a list of lines encrypted to a file.
        /// TODO exceptions
        /// </summary>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="lines">Lines to write to the file.</param>
        /// <param name="cryptoTransform">Transformer that is use to encrypt the data. If this parameter is null, the property value is used.</param>
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
                cryptoTransform = CryptoTransform;

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

        /// <summary>
        /// Writes an object, serialized with json, to a file.
        /// TODO exceptions
        /// </summary>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="value">Value to write to the file as json</param>
        /// <param name="serializer"></param>
        public async Task WriteAsync<T>(T value, string path = null, ISerializer serializer = null)
        {
            if (serializer == null)
                serializer = Serializer;

            path = GetPath<T>(serializer.FileExtension, path);

            using (var fileStream = File.Open(path, FileMode.Create))
            using (var streamWriter = new StreamWriter(fileStream))
                await serializer.SerializeAsync(value, streamWriter);
        }

        /// <summary>
        /// Writes an object, serialized with json, to an encrypted file.
        /// TODO exceptions
        /// </summary>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="value">Value to write to the file as json</param>
        /// <param name="deserializer">Serializer to serialize the object. If this parameter is null, the property value is used.</param>
        /// <param name="cryptoTransform">Transformer that is use to encrypt the data. If this parameter is null, the property value is used.</param>
        public async Task WriteEncryptedAsync<T>(T value, string path = null, ISerializer serializer = null, ICryptoTransform cryptoTransform = null)
        {
            if (serializer == null)
                serializer = Serializer;

            path = GetPath<T>(serializer.FileExtension, path);

            using (var fileStream = File.Open(path, FileMode.Create))
            using (var cryptoStream = new CryptoStream(fileStream, cryptoTransform, CryptoStreamMode.Write))
            using (var streamWriter = new StreamWriter(cryptoStream))
                await serializer.SerializeAsync(value, streamWriter);
        }

        #endregion write

        #endregion METHODS
    }
}
