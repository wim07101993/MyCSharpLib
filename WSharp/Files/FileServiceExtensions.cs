using System.Collections.Generic;
using System.Linq;

using WSharp.Files.Results;

namespace WSharp.Files
{
    public static class FileServiceExtensions
    {
        public static IFileService RegisterReaderWriter<TFile, TReaderWriter>(this IFileService fileService)
            where TFile : class, IFile
            where TReaderWriter : IFileReaderWriter<TFile>
            => fileService.RegisterReaderWriter(typeof(TFile), typeof(TReaderWriter));

        public static List<ISaveFileResult> Save(this IFileService fileService, IEnumerable<IFile> files)
        {
            var list = new List<ISaveFileResult>();

            foreach (var file in files)
                list.Add(fileService.Save(file, file.Path));

            return list;
        }

        public static ISaveFileResult Save(this IFileService fileService, IFile file)
            => fileService.Save(file, file.Path);

        public static ISaveFileResult SaveAs(this IFileService fileService, IFile file)
            => fileService.Save(file, file.Path, true);

        public static T Open<T>(this IFileService fileService, string path)
            => (T)fileService.Open(typeof(T), path);

        public static T Open<T>(this IFileService fileService, bool canOpenMultiple = false)
        {
            var result = fileService.Open(canOpenMultiple, typeof(T));
            return (T)result.Files?.FirstOrDefault();
        }
    }
}
