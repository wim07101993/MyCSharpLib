using System.Threading.Tasks;

namespace MyCSharpLib.Services
{
    public class MockSettingsProvider<T> : ASettingsProvider<T> where T : Settings, new()
    {
        protected override Task<T> InternalFetchSettingsAsync() => Task.FromResult(Settings);
        protected override Task InternalSaveSettingsAsync(T settings) => Task.CompletedTask;
    }
}
