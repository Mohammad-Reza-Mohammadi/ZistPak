using Data.Contracts;
using Entities.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ZPakContext dbContext) : base(dbContext)
        {

        }

        public async Task<OrderDetail> ChangeOrderDetailById(int OrderDetailId, string command, CancellationToken cancellationToken)
        {
            var orderDetail =await base.GetByIdAsync( cancellationToken, OrderDetailId);

            // این شرط چک شود
            switch (command)
            {
                case "up":
                    {
                        orderDetail.CountCargo += 1;
                        break;
                    }
                case "down":
                    {
                        orderDetail.CountCargo -= 1;
                        if (orderDetail.CountCargo == 0)
                        {
                            await base.DeleteAsync(orderDetail, cancellationToken);
                            return null ;

                        }
                        else
                        {
                            await base.UpdateAsync(orderDetail, cancellationToken);
                            return orderDetail;
                        }
                        break;
                    }
            }
            return orderDetail;



        }
    }
}
