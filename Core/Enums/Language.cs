using System.ComponentModel.DataAnnotations;

namespace Core.Enums
{
    public enum Language
    {
        [Display(Name = "English")]
        English = 1,

        [Display(Name = "العربية")]
        Arabic,

        [Display(Name = "Español")]
        Spanish,

        [Display(Name = "Français")]
        French,

        [Display(Name = "German")]
        German,

        [Display(Name = "Italian")]
        Italian,

        [Display(Name = "Japanese")]
        Japanese,

        [Display(Name = "Chinese")]
        Chinese,

        [Display(Name = "Russian")]
        Russian,

        [Display(Name = "Portuguese")]
        Portuguese,

        [Display(Name = "Turkish")]
        Turkish,

        [Display(Name = "Hindi")]
        Hindi,

        [Display(Name = "Korean")]
        Korean,

        [Display(Name = "Urdu")]
        Urdu,

        [Display(Name = "Persian")]
        Persian
    }

}
