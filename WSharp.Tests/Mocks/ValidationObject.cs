using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WSharp.Tests.Mocks
{
    public class ValidationObject
    {
        [Range(5, int.MaxValue)]
        public int GreaterThen5 { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string NotEmpty { get; set; }

        [Required]
        public object NotNull { get; set; }

        public NestedValidationObject Nested { get; set; }

        public List<NestedValidationObject> List { get; set; }
    }
}
