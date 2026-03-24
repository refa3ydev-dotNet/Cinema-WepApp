using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Integration
{
    public class TmdbVideoResult
    {
        public string Id { get; set; }=string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Site { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool Official { get; set; } 
    }
}
