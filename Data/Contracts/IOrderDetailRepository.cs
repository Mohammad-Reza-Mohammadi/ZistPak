using Data.Repositories;
using Entities.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
     public Task<OrderDetail> ChangeOrderDetailById(int OrderDetailId, string command, CancellationToken cancellationToken);
    }


}



