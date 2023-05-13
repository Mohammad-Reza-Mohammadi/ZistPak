using Entities.BaseEntityFolder;

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
