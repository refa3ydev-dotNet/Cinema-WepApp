using System.ComponentModel.DataAnnotations;

namespace Core.Enums
{
    public enum TranslationType
    {
        [Display(Name = "None")]
        None = 1,
        [Display(Name = "Dubbed")]
        Dubbed,
        [Display(Name = "Subtitled")]
        Subtitled
    }
}
