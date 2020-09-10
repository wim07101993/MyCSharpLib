using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

using WSharp.Extensions;

namespace WSharp.Files
{
    public partial class FileService
    {
        private enum EReaderType
        {
            Unknown,
            Json,
            Xml
        }

        private class CustomReaderWriter
        {
            private readonly Func<Stream, IFile> _reader;
            private readonly Func<Stream, Task<IFile>> _readerAsync;

            private readonly Action<IFile, Stream> _writer;
            private readonly Func<IFile, Stream, Task> _writerAsync;

            private readonly IFileReaderWriter _readerWriter;

            public CustomReaderWriter(Func<Stream, IFile> reader = null, Func<Stream, Task<IFile>> readerAsync = null, 
                Action<IFile, Stream> writer = null, Func<IFile, Stream, Task> writerAsync = null)
            {
                ReaderType = EReaderType.Unknown;

                _reader = reader;
                _readerAsync = readerAsync;

                _writer = writer;
                _writerAsync = writerAsync;
            }

            public CustomReaderWriter(IFileReaderWriter readerWriter)
            {
                ReaderType = EReaderType.Unknown;
                _readerWriter = readerWriter;
            }

            public CustomReaderWriter(EReaderType readerType, Type contentType)
            {
                ReaderType = readerType;
                ContentType = contentType;
            }

            public EReaderType ReaderType { get; }
            public Type ContentType { get; }

            public IFile Read(Stream stream)
            {
                if (_reader != null)
                    return _reader(stream);

                if (_readerWriter != null)
                    return _readerWriter.Read(stream);

                return ReaderType switch
                {
                    EReaderType.Json => new StreamReader(stream).DeserializeJson(ContentType),
                    EReaderType.Xml => new XmlSerializer(ContentType).Deserialize(stream),
                    _ => throw new Exception("Unknown reader type, cannot generate reader")
                } as IFile;
            }

            public async Task<IFile> ReadAsync(Stream stream)
            {
                if (_reader != null)
                    return await _readerAsync(stream);

                if (_readerWriter != null)
                    return await _readerWriter.ReadAsync(stream);

                return ReaderType switch
                {
                    EReaderType.Json => new StreamReader(stream).DeserializeJson(ContentType),
                    EReaderType.Xml => new XmlSerializer(ContentType).Deserialize(stream),
                    _ => throw new Exception("Unknown reader type, cannot generate reader")
                } as IFile;
            }

            public void Write(IFile file, Stream stream)
            {
                if (_writer != null)
                {
                    _writer(file, stream);
                    return;
                }

                if (_readerWriter != null)
                {
                    _readerWriter.Write(file, stream);
                    return;
                }

                switch (ReaderType)
                {
                    case EReaderType.Json:
                        new StreamWriter(stream).SerializeJson(file);
                        break;

                    case EReaderType.Xml:
                        new StreamWriter(stream).SerializeXml(file);
                        break;

                    default:
                        throw new Exception("Unknown reader type, cannot generate writer");
                }
            }

            public async Task WriteAsync(IFile file, Stream stream)
            {
                if (_writer != null)
                {
                    await _writerAsync(file, stream);
                    return;
                }

                if (_readerWriter != null)
                {
                    await _readerWriter.WriteAsync(file, stream);
                    return;
                }

                switch (ReaderType)
                {
                    case EReaderType.Json:
                        await new StreamWriter(stream).SerializeJsonAsync(file);
                        break;

                    case EReaderType.Xml:
                        await new StreamWriter(stream).SerializeXmlAsync(file);
                        break;

                    default:
                        throw new Exception("Unknown reader type, cannot generate writer");
                }
            }
        }
    }
}
