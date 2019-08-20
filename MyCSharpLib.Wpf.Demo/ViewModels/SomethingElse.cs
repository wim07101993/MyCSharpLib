using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;

namespace MyCSharpLib.Wpf.Demo.ViewModels
{
    public class SomethingElse : BindableBase
    {
        private string _string;
        private int _int;
        private double _double;


        [Display(Name = "String", Description = "Some string")]
        public string String
        {
            get => _string;
            set => SetProperty(ref _string, value);
        }

        [Display(Name = "Int", Description = "Some int")]
        [Range(0,50)]
        public int Int
        {
            get => _int;
            set => SetProperty(ref _int, value);
        }

        [Display(Name = "Double", Description = "Some double")]
        public double Double
        {
            get => _double;
            set => SetProperty(ref _double, value);
        }
    }
}
