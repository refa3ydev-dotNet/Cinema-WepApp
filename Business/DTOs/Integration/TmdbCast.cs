using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Integration
{
    public class TmdbCast
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;    
        public string Character { get; set; } = string.Empty;
        public string Known_For_Department { get; set; } = string.Empty;
        public string Profile_Path { get; set; } = string.Empty;
        public int Order { get; set; } // ترتيب الممثل في التتر (0 هو البطل الرئيسي)
    }
}
