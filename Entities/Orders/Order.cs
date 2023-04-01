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
        public int userId { get; set; }
        public int sumCrgo { get; set; }
        public bool IsFinaly { get; set; }
        public ICollection<OrderDetail> orderDetails { get; set; }

    }
}
