using Prism.Mvvm;
using System.Threading.Tasks;

namespace MyCSharpLib.Services
{
    public class MockStringsProvider<T> : AStringsProvider<T> where T : IStrings, new()
    {
        public MockStringsProvider() : base(new StringSettings())
        {
        }


        public override string[] Languages
        {
            get => new[] { "eng" };
#pragma warning disable RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
            protected set { }
#pragma warning restore RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
        }

        protected override Task<T> InternalFetchStringsAsync(string language) => Task.FromResult(Strings);
        protected override Task InternalSaveStringsAsync(T strings, string language) => Task.CompletedTask;


        private class StringSettings : BindableBase, IStringsSettings
        {
            public string Language
            {
                get => "eng";
                set { }
            }
        }
    }
}
