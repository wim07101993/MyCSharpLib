using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WSharp.Reflection;

namespace WSharp.Tests.Mocks
{
    public class ValidationObject : AValidatable
    {
        [Range(5, int.MaxValue)]
        public int GreaterThen5 { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string NotEmpty { get; set; }

        [Required]
        public object NotNull { get; set; }

        public NestedValidationObject Nested { get; set; }

        public List<NestedValidationObject> List { get; set; }

        [RegularExpression(@"[1-9][0-9]{0,2}\.[0-9]{1,3}")]
        public string Version { get; set; }
    }
}
