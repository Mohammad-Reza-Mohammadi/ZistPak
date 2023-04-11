using Data.Contracts;
using Entities.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using presentation.Models;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading.Tasks.Dataflow;
using Utility.SwaggerConfig.Permissions;
using WebFramework.Api;
using WebFramework.Filters;
using static Utility.SwaggerConfig.Permissions.Permissions;
using Order = Entities.Orders.Order;

namespace presentation.Controllers
{

    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiResultFilter]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICargoRepository _cargoRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderController(IOrderRepository orderRepository, ICargoRepository cargoRepository, IOrderDetailRepository orderDetailRepository)
        {
            this._orderRepository = orderRepository;
            this._cargoRepository = cargoRepository;
            this._orderDetailRepository = orderDetailRepository;
        }

        [PermissionAuthorize(Permissions.Order.AddToOrder, Admin.admin)]
        [HttpPost]
        public async Task<ApiResult> AddToOrder(int CargoId, CancellationToken cancellationToken)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int currentUserId1 = Convert.ToInt32(currentUserId);

            var cargo = await _cargoRepository.GetByIdAsync(cancellationToken, CargoId);

            if (cargo != null)
            {
                if (cargo.CargoStatus != Entities.Cargo.CargoStatus.Status.Confirm)
                {
                    return Content("محموله تایید نشده است");
                }
                else
                {
                    bool task = await _orderRepository.AddToOrder(currentUserId1, CargoId, cancellationToken);
                    if (task == false)
                    {
                        return Content("مشکلی رخ داده");
                    }
                    return Ok();
                }
            }
            return NotFound();

        }

        [PermissionAuthorize(Permissions.Order.ShowOrder, Admin.admin)]
        [HttpGet]
        public async Task<List<ShowListOrderDto>> ShowOrder(CancellationToken cancellationToken)
        {
            string CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int currentUserId = Convert.ToInt32(CurrentUserId);


            // check the open order for CurrentUserId
            Order order1 = _orderRepository.TableNoTracking.SingleOrDefault(o => o.UserId == currentUserId && !o.IsFinaly);

            List<ShowListOrderDto> OrderList = new List<ShowListOrderDto>();

            if (order1 != null)
            {
                List<OrderDetail> orderDetails = await _orderRepository.GetOrderDetails(order1.Id, cancellationToken);

                foreach (var item in orderDetails)
                {
                    var cargo = _cargoRepository.TableNoTracking.SingleOrDefault(o => o.Id == item.CargoId);
                    OrderList.Add(new ShowListOrderDto()
                    {
                        Count = item.CountCargo,
                        Title = cargo.CargoName,
                        OrderDetailsId = item.Id,
                        Star = item.StarCargo,
                        Sum = item.StarCargo * item.CountCargo,
                    });
                }
            }
            return OrderList;
        }

        [PermissionAuthorize(Permissions.Order.DeleteFromOrder, Admin.admin)]
        [HttpDelete]
        public async Task<ApiResult> DeleteFromOrder(int OrderDetailId, CancellationToken cancellationToken)
        {

            await _orderRepository.DeleteFromOrder(OrderDetailId,cancellationToken);

            return Content("OrderDetails deleted");
        }

        [PermissionAuthorize(Permissions.Order.UpdateOrederDetailInOreder, Admin.admin)]
        [HttpPut]
        public async Task<ApiResult<OrderDetail>> UpdateOrederDetailInOreder(int orderDetailid, string command, CancellationToken cancellation)
        {
            OrderDetail orderDetail = await _orderDetailRepository.ChangeOrderDetailById(orderDetailid, command, cancellation);

            return orderDetail;
        }



    }
}
