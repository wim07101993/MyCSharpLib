using System;

namespace WSharp.Reflection
{
    /// <summary>
    ///     Attribute that describes what authorization a user should have to access the value.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    internal sealed class AuthorizeAttribute : Attribute
    {
        /// <summary>Constructs a new value of the <see cref="AuthorizeAttribute"/>.</summary>
        /// <param name="authorizedLevels">Levels that are allowed to access the value.</param>
        public AuthorizeAttribute(int authorizedLevels)
        {
            AuthorizedLevels = authorizedLevels;
        }

        /// <summary>Levels that are allowed to access the value.</summary>
        public int AuthorizedLevels { get; }
    }
}