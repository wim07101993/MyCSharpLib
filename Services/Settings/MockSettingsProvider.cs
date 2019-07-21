using System.Threading.Tasks;

namespace MyCSharpLib.Services
{
    /// <summary>
    /// Mock class to test classes that depend on the <see cref="ISettingsProvider{T}"/> or <see cref="ISettingsProvider"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MockSettingsProvider<T> : ASettingsProvider<T> where T : Settings, new()
    {
        /// <summary>
        /// Internal method to perform the actual fetching of the settings. (do nothing in this case)
        /// </summary>
        /// <returns>The setting for the application.</returns>
        protected override Task<T> InternalFetchSettingsAsync() => Task.FromResult(Settings);
        /// <summary>
        /// Internal method to perform the actual saving of the settings. (do nothing in this case)
        /// </summary>
        /// <param name="settings">The settings to save.</param>
        protected override Task InternalSaveSettingsAsync(T settings) => Task.CompletedTask;
    }
}
