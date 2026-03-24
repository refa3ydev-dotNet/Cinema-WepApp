using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Integration
{
    public class TmdbCredits
    {
        public List<TmdbCast> Cast { get; set; } = new List<TmdbCast>();
        public List<TmdbCrew> Crew { get; set; } = new List<TmdbCrew>();
    }
}
