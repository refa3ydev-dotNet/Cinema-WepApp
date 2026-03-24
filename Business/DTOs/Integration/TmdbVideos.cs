using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Integration
{
    public class TmdbVideos
    {
        public List<TmdbVideoResult> Results { get; set; } = new List<TmdbVideoResult>();
    }
}
