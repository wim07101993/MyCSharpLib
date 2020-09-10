using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            => files.Select(x => fileService.Save(x, x.Path)).ToList();

        public static async Task<List<ISaveFileResult>> SaveAsync(this IFileService fileService, IEnumerable<IFile> files)
        {
            var tasks = files.Select(x => fileService.SaveAsync(x, x.Path));
            var results = await Task.WhenAll(tasks);
            return results.ToList();
        }

        public static ISaveFileResult Save(this IFileService fileService, IFile file)
            => fileService.Save(file, file.Path);

        public static Task<ISaveFileResult> SaveAsync(this IFileService fileService, IFile file)
            => fileService.SaveAsync(file, file.Path);

        public static ISaveFileResult SaveAs(this IFileService fileService, IFile file)
            => fileService.Save(file, file.Path, true);

        public static Task<ISaveFileResult> SaveAsAsync(this IFileService fileService, IFile file)
            => fileService.SaveAsync(file, file.Path, true);

        public static T Open<T>(this IFileService fileService, string path)
            => (T)fileService.Open(typeof(T), path);

        public static async Task<T> OpenAsync<T>(this IFileService fileService, string path)
            => (T)await fileService.OpenAsync(typeof(T), path);

        public static IMultipleOpenFileResult Open(this IFileService fileService, bool canOpenMultiple, params Type[] types)
            => fileService.Open(canOpenMultiple, types);

        public static Task<IMultipleOpenFileResult> OpenAsync(this IFileService fileService, bool canOpenMultiple, params Type[] types)
            => fileService.OpenAsync(canOpenMultiple, types);

        public static T Open<T>(this IFileService fileService, bool canOpenMultiple = false)
        {
            var result = fileService.Open(canOpenMultiple, typeof(T));
            return (T)result.Files?.FirstOrDefault();
        }
        public static async Task<T> OpenAsync<T>(this IFileService fileService, bool canOpenMultiple = false)
        {
            var result = await fileService.OpenAsync(canOpenMultiple, typeof(T));
            return (T)result.Files?.FirstOrDefault();
        }
    }
}
