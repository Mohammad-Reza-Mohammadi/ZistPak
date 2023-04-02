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
        public string  Name { get; set; }
        public Status status { get; set; }
        public decimal Rating { get; set; }
        public int Count { get; set; }
        public decimal Whight { get; set; }

        #region Relational Property
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        #endregion

    }
}
