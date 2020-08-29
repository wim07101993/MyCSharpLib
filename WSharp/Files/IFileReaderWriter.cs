using System.IO;

using WSharp.Dialogs;

namespace WSharp.Files
{
    public interface IFileReaderWriter<T> : IFileReaderWriter where T : class, IFile
    {
        new T Read(Stream reader);

        void Write(T file, Stream writer);
    }

    public interface IFileReaderWriter
    {
        FileFilter[] FileFilter { get; }

        object Read(Stream reader);

        void Write(object file, Stream writer);
    }
}
