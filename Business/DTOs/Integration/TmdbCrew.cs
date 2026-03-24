using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Integration
{
    public class TmdbCrew
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Job { get; set; }=string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Profile_Path { get; set; } = string.Empty;
    }
}
