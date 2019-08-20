using MyCSharpLib.Services.Logging.Loggers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyCSharpLib.Wpf.Demo.ViewModels
{
    public class ObjectBrowserViewModel : AViewModel
    {
        private string _firstName;
        private string _lastName;
        private int _age;
        private double _salary;
        private Something _something = new Something();

        public ObjectBrowserViewModel(ILogDispatcher logger)
            : base(logger)
        {
        }

        [Display(Name = "First name", Description = "First name of the employee")]
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        [Display(Name = "Last name", Description = "Last name of the employee")]
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        [Display(Name = "Age", Description = "Number of years the employee has lived until now.")]
        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        [Display(Name = "Salary", Description = "Amount of money the employee makes in a month.")]
        public double Salary
        {
            get => _salary;
            set => SetProperty(ref _salary, value);
        }

        [Description("I didnt know any name to give to this property")]
        public Something Something
        {
            get => _something;
            set => SetProperty(ref _something, value);
        }
    }
}
