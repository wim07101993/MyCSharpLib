using System;
using System.Collections.Generic;

using WSharp.Files.Results;

namespace WSharp.Files
{
    public interface IFileService
    {
        IReadOnlyDictionary<Type, IFileReaderWriter> ReaderWriters { get; }

        IFileService RegisterReaderWriter(Type type, IFileReaderWriter readerWirter);

        IFileService RegisterReaderWriter(Type fileType, Type readerWriterType);

        ISaveFileResult Save(IFile file, string path, bool forceDialog = false);

        IMultipleOpenFileResult Open(bool canOpenMultiple, params Type[] types);

        IFile Open(Type type, string filePath);

        IMultipleCloseFileResult Close(params IFile[] filesToClose);
    }
}
