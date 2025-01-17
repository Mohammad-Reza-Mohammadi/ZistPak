﻿using Data.Contracts;
using Entities.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using presentation.Models;
using System.Security.Claims;
using WebFramework.Api;
using WebFramework.Filters;
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
        private readonly IUserRepository _userRepository;

        public OrderController(IOrderRepository orderRepository, ICargoRepository cargoRepository, IOrderDetailRepository orderDetailRepository,IUserRepository userRepository)
        {
            this._orderRepository = orderRepository;
            this._cargoRepository = cargoRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._userRepository = userRepository;
        }

        /// <summary>
        /// اضافه کردن محموله به سبد خرید
        /// </summary>
        /// <param name="CargoId">آیدی محموله</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
            return Content("چنین محموله ای یافت نشد .");

        }

        /// <summary>
        /// نمایش سبد خرید
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        /// <summary>
        /// حذف محموله از سبد خرید
        /// </summary>
        /// <param name="OrderDetailId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult> DeleteFromOrder(int OrderDetailId, CancellationToken cancellationToken)
        {

            await _orderRepository.DeleteFromOrder(OrderDetailId,cancellationToken);

            return Content("OrderDetails deleted");
        }

        /// <summary>
        /// تغییر تعداد محموله در سبد خرید
        /// </summary>
        /// <param name="orderDetailid">ایدی فاکتور جزئیات مربوط به محموله</param>
        /// <param name="command">دستور تغییر</param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult<OrderDetail>> UpdateOrederDetailInOreder(int orderDetailid, string command, CancellationToken cancellation)
        {
            OrderDetail orderDetail = await _orderDetailRepository.ChangeOrderDetailById(orderDetailid, command, cancellation);

            return orderDetail;
        }

        /// <summary>
        /// نهایی کردن سبد خرید
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> FinalizeThePurchaseAsync(CancellationToken cancellation)
        {
            string CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int currentUserId = Convert.ToInt32(CurrentUserId);

            await _userRepository.FinalizeThePurchaseAsync(currentUserId,cancellation);
            return Ok();
            
        }


    }
}
