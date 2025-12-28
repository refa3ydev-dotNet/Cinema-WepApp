using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Director
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public string Biography { set; get; }
        public string? ProfilePicture { get; set; }
        public DateOnly?BirthDate { get; set; }
        public string? IMDB { get; set; }
        // Relations
        public List<Movie>? Movies { get; set; }



    }
}
