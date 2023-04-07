using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.User.Owned
{
    [Owned]
    public class FullName
    {
        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set; }
    }
}
