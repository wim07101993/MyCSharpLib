using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WSharp.Files.Results;

namespace WSharp.Files
{
    public interface IFileService
    {
        IReadOnlyDictionary<Type, IFileReaderWriter> ReaderWriters { get; }

        IFileService RegisterReaderWriter(Type type, IFileReaderWriter readerWirter);

        IFileService RegisterReaderWriter(Type fileType, Type readerWriterType);

        ISaveFileResult Save(IFile file, string path, bool forceDialog = false);
        Task<ISaveFileResult> SaveAsync(IFile file, string path, bool forceDialog = false);

        IMultipleOpenFileResult Open(bool canOpenMultiple, IEnumerable<Type> types);
        Task<IMultipleOpenFileResult> OpenAsync(bool canOpenMultiple, IEnumerable<Type> types);

        IFile Open(Type type, string filePath);
        Task<IFile> OpenAsync(Type type, string filePath);

        IMultipleCloseFileResult Close(IList<IFile> filesToClose);
        Task<IMultipleCloseFileResult> CloseAsync(IList<IFile> filesToClose);
    }
}
