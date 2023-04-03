using Data.Repositories;
using Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface IBasketRepository:IRepository<Order>
    {
        Task<bool> AddToCart(int CurrentUserId, int Id, CancellationToken cancellationToken);
        Task<List<OrderDetail>> GetOrderDetails(int OrderDetailsId ,CancellationToken cancellationToken);
        public Task<bool> DeleteFromOrder(int OrderDeratilsId, CancellationToken cancellationToken);
    }
}
