using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;

namespace WSharp.Wpf.Demo.ViewModels
{
    public class Something : BindableBase
    {
        private string _string;
        private int _int;
        private double _double;
        private SomethingElse _somethingElse;


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


        public SomethingElse SomethingElse
        {
            get => _somethingElse;
            set => SetProperty(ref _somethingElse, value);
        }
    }
}
