﻿using Data.Contracts;
using Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Order = Entities.Orders.Order;

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

                            var orderId = orderDetail.OrderId;

                            IEnumerable<OrderDetail> orderDetails = DbContext.Set<OrderDetail>().Where(od => od.OrderId == orderId);

                            var sum = orderDetails.Sum(od => od.CountCargo * od.StarCargo);

                            var order = await DbContext.Set<Order>().Where(o=>o.Id == orderId).FirstOrDefaultAsync();
                            order.OrderStar = sum;
                            DbContext.Update(order);
                            await DbContext.SaveChangesAsync();
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

        public async Task<OrderDetail> GetByOrderId(int orderId,int orederDetailId, CancellationToken cancellationToken)
        {
            return await DbContext.Set<OrderDetail>().FirstOrDefaultAsync(od => od.OrderId == orderId && od.Id == orederDetailId, cancellationToken);
        }
    }
}
