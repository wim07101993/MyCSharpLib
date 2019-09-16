using WSharp.Services.Serialization;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace WSharp.Services
{
    /// <summary>
    /// Interface that describes the methods and properties of a file service
    /// </summary>
    public interface IFileService
    {
        /// <summary>Serializer used to serialize objects to write to a file.</summary>
        ISerializer Serializer { get; }

        /// <summary>Deserializer used to deserialize objects read from a file.</summary>
        IDeserializer Deserializer { get; }

        /// <summary>Transformer used to encrypt and decrypt data to write and read from encrypted files.</summary>
        ICryptoTransform CryptoTransform { get; }



        /// <summary>
        /// Generates a new data file path for type T.
        /// {data directory}\{type name}.{extension}
        /// </summary>
        /// <typeparam name="T">Type for which the file is ment.</typeparam>
        /// <param name="extension">The extension of the file</param>
        /// <returns>The path to write a file to.</returns>
        string GenerateDataFilePath<T>(string extension);

        #region read

        /// <summary>
        /// Read the text from a specified path as text.
        /// </summary>
        /// <param name="path">Path to the file to read the content from.</param>
        /// <returns>The string content of the file.</returns>
        Task<string> ReadTextAsync(string path);

        /// <summary>
        /// Read the encrypted text from a specified encrypted path.
        /// </summary>
        /// <param name="path">Path to the file to read the content from.</param>
        /// <param name="cryptoTransform">Transformer that is use to decrypt the data. If this parameter is null, the property value is used.</param>
        /// <returns>The string content of the file.</returns>
        Task<string> ReadEncryptedTextAsync(string path, ICryptoTransform cryptoTransform = null);

        /// <summary>
        /// Reads all lines from a specified file.
        /// </summary>
        /// <param name="path">Path to the file to read from.</param>
        /// <returns>The lines of the file.</returns>
        IEnumerable<string> ReadLines(string path);

        /// <summary>
        /// Reads all lines from a specified encrypted file.
        /// </summary>
        /// <param name="path">Path to the file to read from.</param>
        /// <param name="cryptoTransform">Transformer that is use to decrypt the data. If this parameter is null, the property value is used.</param>
        /// <returns>The lines of the file.</returns>
        IEnumerable<string> ReadEncryptedLines(string path, ICryptoTransform cryptoTransform = null);

        /// <summary>
        /// Reads the content of a file and parses it to an object.
        /// </summary>
        /// <typeparam name="T">Type to parse the content to.</typeparam>
        /// <param name="path">Path to the file to read the json from.</param>
        /// <param name="deserializer">Deserializer to deserialize the content of the file. If this parameter is null, the property value is used.</param>
        /// <returns>The parsed value.</returns>
        Task<T> ReadAsync<T>(string path = null, IDeserializer deserializer = null);

        /// <summary>
        /// Reads the encrypted content of a file and parses it to an object.
        /// </summary>
        /// <typeparam name="T">Type to parse the content to.</typeparam>
        /// <param name="path">Path to the file to read the json from.</param>
        /// <param name="deserializer">Deserializer to deserialize the content of the file. If this parameter is null, the property value is used.</param>
        /// <param name="cryptoTransform">Transformer that is use to decrypt the data. If this parameter is null, the property value is used.</param>
        /// <returns>The parsed value.</returns>
        Task<T> ReadEncryptedAsync<T>(string path = null, IDeserializer deserializer = null, ICryptoTransform cryptoTransform = null);

        #endregion read

        #region write

        /// <summary>
        /// Writes a string to a file.
        /// </summary>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="text">Text to write to the file.</param>
        Task WriteTextAsync(string text, string path);

        /// <summary>
        /// Writes a string encrypted to a file.
        /// </summary>
        /// <param name="text">Text to write to the file.</param>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="cryptoTransform">Transformer that is use to encrypt the data. If this parameter is null, the property value is used.</param>
        Task WriteEncryptedTextAsync(string text, string path, ICryptoTransform cryptoTransform = null);

        /// <summary>
        /// Writes a list of lines to a file.
        /// </summary>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="lines">Lines to write to the file.</param>
        Task WriteLinesAsync(IEnumerable<string> lines, string path);

        /// <summary>
        /// Writes a list of lines encrypted to a file.
        /// </summary>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="lines">Lines to write to the file.</param>
        /// <param name="cryptoTransform">Transformer that is use to encrypt the data. If this parameter is null, the property value is used.</param>
        Task WriteEncryptedLinesAsync(IEnumerable<string> lines, string path, ICryptoTransform cryptoTransform = null);

        /// <summary>
        /// Writes an object, serialized with json, to a file.
        /// </summary>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="value">Value to write to the file as json</param>
        /// <param name="serializer"></param>
        Task WriteAsync<T>(T value, string path = null, ISerializer serializer = null);

        /// <summary>
        /// Writes an object, serialized with json, to an encrypted file.
        /// </summary>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="value">Value to write to the file as json</param>
        /// <param name="deserializer">Serializer to serialize the object. If this parameter is null, the property value is used.</param>
        /// <param name="cryptoTransform">Transformer that is use to encrypt the data. If this parameter is null, the property value is used.</param>
        Task WriteEncryptedAsync<T>(T value, string path = null, ISerializer serializer = null, ICryptoTransform cryptoTransform = null);

        #endregion write
    }
}