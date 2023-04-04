using Data.Contracts;
using ECommerce.Utility;
using Entities.Cargo;
using Entities.Orders;
using Entities.Useres;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class BasketRepository : Repository<Order>, IBasketRepository
    {
        public BasketRepository(ZPakContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddToBasket(int CurrentUserId, int Id, CancellationToken cancellationToken)
        {
            //bool status = false;
            Order order = DbContext.Set<Order>().SingleOrDefault(o => o.userId == CurrentUserId && !o.IsFinaly);

            if (order == null)
            {
                order = new Order();
                order.userId = CurrentUserId;
                order.CreateDate = DateTime.Now.ToString();
                order.IsFinaly = false;
                order.RatingOreder = 0;
                await DbContext.Set<Order>().AddAsync(order);
                await DbContext.SaveChangesAsync();

                var caro = await DbContext.Set<Cargo>().SingleOrDefaultAsync(p => p.Id == Id);
                int Rating = Convert.ToInt32(caro.Rating);

                await DbContext.Set<OrderDetail>().AddAsync(new OrderDetail()
                {

                    CreateDate = DateTime.Now.ToShamsi(),
                    CountCargo = 1,
                    OrderId = order.Id,
                    rating = Rating,
                    cargoId = Id,

                });
                await DbContext.SaveChangesAsync();
            }
            else
            {
                var details = await DbContext.Set<OrderDetail>().SingleOrDefaultAsync(o => o.OrderId == order.Id && o.cargoId == Id);
                if (details == null)
                {
                    var cargo = await DbContext.Set<Cargo>().SingleOrDefaultAsync(p => p.Id == Id);

                    int Rating = Convert.ToInt32(cargo.Rating);

                    await DbContext.Set<OrderDetail>().AddAsync(new OrderDetail()
                    {
                        CreateDate = DateTime.Now.ToShamsi(),
                        CountCargo = 1,
                        OrderId = order.Id,
                        rating = Rating,
                        cargoId = Id,
                    }) ;
                }
                else
                {
                    details.CountCargo += 1;
                    DbContext.Update(details);
                }
                await DbContext.SaveChangesAsync();
            }
            await UpdateSum(order.Id, cancellationToken);
            return true;
        }

        async Task UpdateSum(int orderId, CancellationToken cancellationToken)
        {
            var order = await base.GetByIdAsync(cancellationToken, orderId);
            var count = await DbContext.Set<OrderDetail>().Where(o => o.OrderId == orderId).Select(d => d.CountCargo).FirstOrDefaultAsync();
            var rating = await DbContext.Set<OrderDetail>().Where(o => o.OrderId == orderId).Select(d => d.rating).FirstOrDefaultAsync();

            var sum = count * rating;
            order.RatingOreder = sum;
            DbContext.Update(order);
            await DbContext.SaveChangesAsync();

        }

        public async Task<List<OrderDetail>> GetOrderDetails(int OrderDetailsId, CancellationToken cancellationToken)
        {
            var Orderdetails =  DbContext.Set<OrderDetail>().Where(o => o.OrderId == OrderDetailsId).ToList();
            if (Orderdetails == null)
            {
                return null;
            }
            return Orderdetails;
        }

        public async Task<bool> DeleteFromOrder(int OrderDeratilsId, CancellationToken cancellationToken)
        {
            var orderDetail= await DbContext.Set<OrderDetail>().FindAsync(cancellationToken, OrderDeratilsId);
            DbContext.Set<OrderDetail>().Remove(orderDetail);
            return true;
        }
    }
}

