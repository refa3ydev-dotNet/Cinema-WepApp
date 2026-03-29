using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Integration
{

        public class TmdbPersonDetails
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Biography { get; set; }
            public string Place_Of_Birth { get; set; } // TMDB بيستخدم مكان الميلاد كبديل للجنسية
            public string Birthday { get; set; } // بييجي بصيغة "YYYY-MM-DD"
            public string Deathday { get; set; }
    }
    }

