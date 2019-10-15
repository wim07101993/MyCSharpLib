using System;

namespace WSharp.Reflection
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    internal sealed class AuthorizeAttribute : Attribute
    {
        public AuthorizeAttribute(int authorizedLevels)
        {
            AuthorizedLevels = authorizedLevels;
        }

        public int AuthorizedLevels { get; }
    }
}