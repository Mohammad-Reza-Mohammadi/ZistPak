using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ModelsDto.User
{
    public class LoginRequest
    {
        [Required]
        public string grant_type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        //public string refresh_token { get; set; }
        //public string scope { get; set; }

        public string? client_id { get; set; }
        public string? client_secret { get; set; }
    }
}
