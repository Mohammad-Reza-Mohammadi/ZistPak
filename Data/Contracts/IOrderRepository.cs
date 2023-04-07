using Data.Repositories;
using Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface IOrderRepository:IRepository<Order>
    {
        Task<bool> AddToOrder(int CurrentUserId, int Id, CancellationToken cancellationToken);
        Task<List<OrderDetail>> GetOrderDetails(int orderId, CancellationToken cancellationToken);
        public Task<bool> DeleteFromOrder(int OrderDeratilsId, CancellationToken cancellationToken);
    }
}
