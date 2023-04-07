using Entities.BaseEntityFolder;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Orders
{
    public class Order:IntBaseEntity
    {
        public int UserId { get; set; }
        public decimal OrderStar { get; set; }//قیمت کل سبد خرید
        public bool IsFinaly { get; set; }
        public virtual ICollection<OrderDetail> orderDetails { get; set; }

    }
}
