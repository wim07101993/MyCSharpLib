using System.IO;
using System.Threading.Tasks;

using WSharp.Dialogs;

namespace WSharp.Files
{
    public interface IFileReaderWriter<T> : IFileReaderWriter where T : class, IFile
    {
        new T Read(Stream reader);

        new Task<T> ReadAsync(Stream reader);

        void Write(T file, Stream writer);

        Task WriteAsync(T file, Stream writer);
    }

    public interface IFileReaderWriter
    {
        FileFilter[] FileFilter { get; }

        IFile Read(Stream reader);

        Task<IFile> ReadAsync(Stream reader);

        void Write(IFile file, Stream writer);

        Task WriteAsync(IFile file, Stream writer);
    }
}
