using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    class RequiredListAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var list = value as IList<int>;
            return list != null && list.Any();
        }
    }
}
