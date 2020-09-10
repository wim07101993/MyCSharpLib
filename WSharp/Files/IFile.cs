using System;

namespace WSharp.Files
{
    public interface IFile : ISavedIndicator, IDisposable
    {
        string Path { get; set; }
    }
}
