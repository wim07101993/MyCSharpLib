using Unity;

namespace WSharp
{
    public interface IWithUnityContainer
    {
        IUnityContainer UnityContainer { get; }
    }
}
