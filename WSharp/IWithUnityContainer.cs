using Unity;

namespace WSharp
{
    /// <summary>Object with a <see cref="IUnityContainer"/> property.</summary>
    public interface IWithUnityContainer
    {
        /// <summary>The <see cref="IUnityContainer"/> used to inject dependencies.</summary>
        IUnityContainer UnityContainer { get; }
    }
}