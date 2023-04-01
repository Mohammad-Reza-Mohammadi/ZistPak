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
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string AddressTitle { get; set; }
      //public enum Title { get; set; }
        public string City { get; set; }
        public string? Town { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
    }
}
