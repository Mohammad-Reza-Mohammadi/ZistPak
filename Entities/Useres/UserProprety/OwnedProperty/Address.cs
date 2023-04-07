using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Entities.User.Owned
{
    public class Address
    {
        public int UserAddressId { get; set; }
        public int UserAddressOwnerId { get; set; }
        public string UserAddressTitle { get; set; }
        //public enum Title { get; set; }
        public string UserAddressCity { get; set; }
        public string? UserAddressTown { get; set; }
        public string? UserAddressStreet { get; set; }
        public string? UserAddressPostalCode { get; set; }
    }
}
