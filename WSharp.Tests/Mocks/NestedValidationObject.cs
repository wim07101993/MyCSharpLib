using System.ComponentModel.DataAnnotations;

namespace WSharp.Tests.Mocks
{
    public class NestedValidationObject
    {
        [Range(5, int.MaxValue)]
        public int GreaterThen5 { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string NotEmpty { get; set; }

        [Required]
        public object NotNull { get; set; }
    }
}