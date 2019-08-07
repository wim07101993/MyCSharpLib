using Unity;

namespace MyCSharpLib
{
    public interface IWithUnityContainer
    {
        IUnityContainer UnityContainer { get; }
    }
}
