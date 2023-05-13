using Data.Contracts;
using ECommerce.Utility;
using Entities.Orders;
using Microsoft.EntityFrameworkCore;
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
                    Star = cargoStar

                }) ;
                await DbContext.SaveChangesAsync();

            }
            else
            {
                var details = await DbContext.Set<OrderDetail>().SingleOrDefaultAsync(o => o.OrderId == order.Id && o.CargoId == cargoId);
                if (details == null)
                {
                    Cargo cargo = await _cargoRepository.GetByIdAsync(cancellationToken, cargoId);
                    decimal cargoStar = cargo.CargoStar;

                    await DbContext.Set<OrderDetail>().AddAsync(new OrderDetail()
                    {
                        CreateDate = DateTime.Now.ToShamsi(),
                        CountCargo = 1,
                        OrderId = order.Id,
                        StarCargo = cargoStar,
                        CargoId = cargoId,
                        Star = cargoStar,
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

            var order = await base.GetByIdAsync(cancellationToken, orderId);
            //var count =  DbContext.Set<OrderDetail>().Where(od => od.OrderId == orderId).Select(d=>d.CountCargo).First();
            var orderDetails =  DbContext.Set<OrderDetail>().Where(od => od.OrderId == orderId);
            foreach(var orderDetail in orderDetails)
            {
                var cargoStar = orderDetail.StarCargo;
                var cargoCount = orderDetail.CountCargo;
                orderDetail.Star = cargoStar * cargoCount;
                DbContext.Update(orderDetail);
            }
                DbContext.SaveChanges();
            var sumOrderDetailStar = orderDetails.Sum(od => od.Star);
            
            order.OrderStar = sumOrderDetailStar;
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

