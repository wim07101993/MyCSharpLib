using System;
using System.Collections.Generic;

namespace WSharp.Files
{
    public class ReaderWriterCollection : Dictionary<Type, IFileReaderWriter>, IReaderWriterCollection
    {
    }
}
