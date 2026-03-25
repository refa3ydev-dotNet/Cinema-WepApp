using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Accounts
{
    public class StaySignedInDto
    {
        public string Email { get; set; }=string.Empty;
        public bool Rememberme { get; set; }
        public bool Dontshowagain { get; set; }
    }
}
