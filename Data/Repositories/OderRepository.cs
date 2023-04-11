using Data.Contracts;
using ECommerce.Utility;
using Entities.Cargo;
using Entities.Orders;
using Entities.Useres;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Utility.SwaggerConfig.Permissions.Permissions;
using Cargo = Entities.Cargo.Cargo;
using Order = Entities.Orders.Order;

namespace Data.Repositories
{
    public class OderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ICargoRepository _cargoRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OderRepository(ZPakContext dbContext, ICargoRepository cargoRepository,IOrderDetailRepository orderDetailRepository) : base(dbContext)
        {
            this._cargoRepository = cargoRepository;
            this._orderDetailRepository = orderDetailRepository;
        }

        public async Task<bool> AddToOrder(int currentUserId, int cargoId, CancellationToken cancellationToken)
        {
            Order order = await Table.Where(o => o.UserId == currentUserId && o.IsFinaly == false).SingleOrDefaultAsync(cancellationToken);

            if (order == null)
            {
                order = new Order();
                order.UserId = currentUserId;
                order.CreateDate = DateTime.Now.ToString();
                order.IsFinaly = false;
                order.OrderStar = 0;
                await base.AddAsync(order, cancellationToken);// AddAsync have SaveChange Mehtod

                Cargo cargo = await _cargoRepository.GetByIdAsync(cancellationToken, cargoId);

                decimal cargoStar = cargo.CargoStar;

                await DbContext.Set<OrderDetail>().AddAsync(new OrderDetail()
                {

                    CreateDate = DateTime.Now.ToShamsi(),
                    CountCargo = 1,
                    StarCargo = cargoStar,
                    OrderId = order.Id,
                    CargoId = cargoId,

                });
                await DbContext.SaveChangesAsync();

            }
            else
            {
                var details = await DbContext.Set<OrderDetail>().SingleOrDefaultAsync(o => o.OrderId == order.Id && o.CargoId == cargoId);
                if (details == null)
                {
                    Cargo cargo = await _cargoRepository.GetByIdAsync(cancellationToken,cargoId);
                    decimal cargoStar = cargo.CargoStar;

                    await DbContext.Set<OrderDetail>().AddAsync(new OrderDetail()
                    {
                        CreateDate = DateTime.Now.ToShamsi(),
                        CountCargo = 1,
                        OrderId = order.Id,
                        StarCargo = cargoStar,
                        CargoId = cargoId,
                    });

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
            Order order = await base.GetByIdAsync(cancellationToken, orderId);
            OrderDetail orderDetail = await DbContext.Set<OrderDetail>().Where(od=>od.OrderId == orderId).SingleOrDefaultAsync();

            int countCargoInOrderdetail = orderDetail.CountCargo;
            decimal starCargo = orderDetail.StarCargo;

            var sum = countCargoInOrderdetail * starCargo;

            order.OrderStar = sum;
            DbContext.Update(order);
            await DbContext.SaveChangesAsync();

        }

        public async Task<List<OrderDetail>> GetOrderDetails(int orderId, CancellationToken cancellationToken)
        {
            List<OrderDetail> orderDetails = DbContext.Set<OrderDetail>().Where(o => o.OrderId == orderId).ToList();

            if (orderDetails == null)
            {
                return null;
            }
            return orderDetails;
        }

        public async Task<bool> DeleteFromOrder(int OrderDeratilsId, CancellationToken cancellationToken)
        {
            OrderDetail orderDetail = await _orderDetailRepository.GetByIdAsync(cancellationToken, OrderDeratilsId);
            DbContext.Set<OrderDetail>().Remove(orderDetail);
            await DbContext.SaveChangesAsync();

            var orderId = orderDetail.OrderId;

            IEnumerable<OrderDetail> orderDetails = DbContext.Set<OrderDetail>().Where(od => od.OrderId == orderId);

            var sum = orderDetails.Sum(od => od.CountCargo * od.StarCargo);

            Order order = await base.GetByIdAsync(cancellationToken, orderId);
            order.OrderStar = sum;
            DbContext.Update(order);
            await DbContext.SaveChangesAsync();

            return true;
        }
    }
}

