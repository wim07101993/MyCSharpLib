using System;
using System.Collections.Generic;

namespace WSharp.Files
{
    public interface IReaderWriterCollection : IDictionary<Type, IFileReaderWriter>
    {
    }
}
