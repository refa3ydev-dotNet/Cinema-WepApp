using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Integration
{
    public class TmdbProductionsCompany
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Logo_Path { get; set; } = string.Empty;
        public string Origin_Country { get; set; } = string.Empty;
    }
}
