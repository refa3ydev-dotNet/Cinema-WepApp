using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum MovieCategory
    {
        [Display(Name = "Action")]
        Action = 1,
        [Display(Name = "Comedy")]
        Comedy,
        [Display(Name = "Drama")]
        Drama,
        [Display(Name = "Documentary")]
        Documentary,
        [Display(Name = "Horror")]
        Horror,
        [Display(Name = "Romance")]
        Romance,
        [Display(Name = "Crime")]
        Crime,
        [Display(Name = "Animation")]
        Cartoon,
        [Display(Name = "Sci-Fi")]
        SciFi,
        [Display(Name = "Adventure")]
        Adventure,
        [Display(Name = "Thriller")]
        Thriller,
        [Display(Name = "Mystery")]
        Mystery,
        [Display(Name = "Fantasy")]
        Fantasy,
        [Display(Name = "Animation")]
        Animation,
        [Display(Name = "Musical")]
        Musical,
        [Display(Name = "Western")]
        Western,
        [Display(Name = "Historical")]
        Historical,
        [Display(Name = "War")]
        War,
        [Display(Name = "Biography")]
        Biography,
        [Display(Name = "Crime")]
        Sport,
        [Display(Name = "Family")]
        Family,
        [Display(Name = "Superhero")]
        Superhero,
        [Display(Name = "Reality")]
        Short,
        [Display(Name = "Independent")]
        Independent
    }

}
