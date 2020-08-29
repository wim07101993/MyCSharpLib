using System;
using System.Collections.Generic;

namespace WSharp.Dialogs
{
    public struct FileFilter : IEquatable<FileFilter>
    {
        public FileFilter(string extensions, string fileTypeName, Type fileContentsType)
        {
            Extensions = extensions;
            FileTypeName = fileTypeName;
            FileContentsType = fileContentsType;
        }

        public string Extensions { get; set; }
        public string FileTypeName { get; set; }
        public Type FileContentsType { get; set; }

        public override bool Equals(object obj) => obj is FileFilter filter && Equals(filter);

        public bool Equals(FileFilter other)
        {
            return (FileContentsType, FileTypeName, Extensions) ==
                (other.FileContentsType, other.FileTypeName, other.Extensions);
        }

        public override int GetHashCode()
        {
            var hashCode = 1132001112;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Extensions);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(FileTypeName);
            hashCode = (hashCode * -1521134295) + EqualityComparer<Type>.Default.GetHashCode(FileContentsType);
            return hashCode;
        }
    }
}
