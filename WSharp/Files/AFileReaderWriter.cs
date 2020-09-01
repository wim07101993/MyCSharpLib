using System.IO;
using System.Threading.Tasks;

using WSharp.Dialogs;

namespace WSharp.Files
{
    public abstract class AFileReaderWriter<T> : IFileReaderWriter<T> where T : class, IFile
    {
        public abstract FileFilter[] FileFilter { get; }

        public abstract T Read(Stream reader);
        public abstract Task<T> ReadAsync(Stream reader);

        IFile IFileReaderWriter.Read(Stream reader) => Read(reader);
        async Task<IFile> IFileReaderWriter.ReadAsync(Stream reader) => await ReadAsync(reader);

        public abstract void Write(T file, Stream writer);
        public abstract Task WriteAsync(T file, Stream writer);

        void IFileReaderWriter.Write(IFile file, Stream writer) => Write((T)file, writer);
        Task IFileReaderWriter.WriteAsync(IFile file, Stream writer) => WriteAsync((T)file, writer);
    }
}
