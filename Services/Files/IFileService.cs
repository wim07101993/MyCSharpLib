using MyCSharpLib.Services.Serialization;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MyCSharpLib.Services
{
    /// <summary>
    /// Interface that describes the methods and properties of a file service
    /// </summary>
    public interface IFileService
    {
        string LogDirectory { get; }


        string GenerateDataFilePath<T>(string extension);

        #region read

        /// <summary>
        /// Read the text from a specified path asynchronously
        /// </summary>
        /// <param name="path">Path to the file to read the content from</param>
        /// <returns>The string content of the file</returns>
        Task<string> ReadTextAsync(string path);

        /// <summary>
        /// Reads all lines from a specified file.
        /// </summary>
        /// <param name="path">Path to the file to read from.</param>
        /// <returns>The lines of the file.</returns>
        IEnumerable<string> ReadLines(string path);

        /// <summary>
        /// Reads the content of a file and parses it to an object.
        /// </summary>
        /// <typeparam name="T">Type to parse the content to.</typeparam>
        /// <param name="path">Path to the file to read the json from.</param>
        /// <param name="deserializer">Deserializer to deserialize the content of the file. If this parameter is null, the default is used.</param>
        /// <returns>The parsed value.</returns>
        Task<T> ReadAsync<T>(string path = null, IDeserializer deserializer = null);

        /// <summary>
        /// Read the encrypted text from a specified path.
        /// </summary>
        /// <param name="path">Path to the file to read the content from</param>
        /// <param name="cryptoTransform">Transformer that is use to decrypt the data.</param>
        /// <returns>The string content of the file</returns>
        Task<string> ReadEncryptedTextAsync(string path, ICryptoTransform cryptoTransform = null);

        /// <summary>
        /// Reads all lines from a specified file.
        /// </summary>
        /// <param name="path">Path to the file to read from.</param>
        /// <param name="cryptoTransform">Transformer that is use to decrypt the data.</param>
        /// <returns>The lines of the file.</returns>
        IEnumerable<string> ReadEncryptedLines(string path, ICryptoTransform cryptoTransform = null);

        /// <summary>
        /// Reads the encrypted content of a file and parses it to an object.
        /// </summary>
        /// <typeparam name="T">Type to parse the content to.</typeparam>
        /// <param name="path">Path to the file to read the json from.</param>
        /// <param name="deserializer">Deserializer to deserialize the content of the file. If this parameter is null, the default is used.</param>
        /// <param name="cryptoTransform">Transformer that is use to decrypt the data.</param>
        /// <returns>The parsed value.</returns>
        Task<T> ReadEncryptedAsync<T>(string path = null, IDeserializer deserializer = null, ICryptoTransform cryptoTransform = null);

        #endregion read

        #region write

        /// <summary>
        /// Writes a string to a file asynchronously.
        /// </summary>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="text">Text to write to the file.</param>
        Task WriteTextAsync(string text, string path);

        /// <summary>
        /// Writes a string encrypted to a file.
        /// </summary>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="text">Text to write to the file.</param>
        Task WriteEncryptedTextAsync(string text, string path, ICryptoTransform cryptoTransform = null);

        /// <summary>
        /// Writes a list of lines to a file asynchronoulsy.
        /// </summary>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="lines">Lines to write to the file.</param>
        Task WriteLinesAsync(IEnumerable<string> lines, string path);

        /// <summary>
        /// Writes a list of lines encrypted to a file.
        /// </summary>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="lines">Lines to write to the file.</param>
        Task WriteEncryptedLinesAsync(IEnumerable<string> lines, string path, ICryptoTransform cryptoTransform = null);

        /// <summary>
        /// Writes an object, serialized with json, to a file.
        /// </summary>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="value">Value to write to the file as json</param>
        Task WriteAsync<T>(T value, string path = null, ISerializer serializer = null);

        Task WriteEncryptedAsync<T>(T value, string path = null, ISerializer serializer = null, ICryptoTransform cryptoTransform = null);

        #endregion write
    }
}