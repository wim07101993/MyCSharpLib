using System.ComponentModel;

namespace MyCSharpLib.Services
{
    /// <summary>
    /// Interface to determine what settings are needed for the <see cref="StringsProvider{T}"/>.
    /// </summary>
    public interface IStringsSettings : INotifyPropertyChanged
    {
        /// <summary>The current language of the application.</summary>
        string Language { get; set; }
    }
}
