using Entities.BaseEntityFolder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Orders
{
    public class OrderDetail : IntBaseEntity
    {

        public int rating { get; set; }
        public int CountCargo { get; set; }


        public int OrderId { get; set; }
        public int cargoId { get; set; }

        public Order Order { get; set; }
        public Cargo.Cargo cargo { get; set; }
    }
}
