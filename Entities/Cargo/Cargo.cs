using Entities.BaseEntityFolder;
using Entities.Cargo.CargoStatus;
using Entities.Orders;
using Entities.User;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Cargo
{
    public class Cargo:IntBaseEntity
    {
        public string  CargoName { get; set; }
        public Status CargoStatus { get; set; }
        public decimal CargoStar { get; set; }
        public int ItemCount { get; set; }
        public decimal CargoWhight { get; set; }

        #region Relational Property
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        #endregion

    }
}
