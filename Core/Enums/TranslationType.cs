using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum TranslationType
    {
        [Display(Name = "None")]
        None=1,
        [Display(Name = "Dubbed")]
        Dubbed,
        [Display(Name = "Subtitled")]
        Subtitled
    }
}
