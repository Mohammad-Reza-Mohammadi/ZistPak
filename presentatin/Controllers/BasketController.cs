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

namespace presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly ICargoRepository cargoRepository;

        public BasketController(IBasketRepository basketRepository,ICargoRepository cargoRepository)
        {
            this.basketRepository = basketRepository;
            this.cargoRepository = cargoRepository;
        }

        [PermissionAuthorize(Permissions.Basket.AddToCart)]
        [HttpPost]
        public async Task<ActionResult> AddToCart(int CargoId,CancellationToken cancellationToken)
        {
            string CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int currentUserId = Convert.ToInt32(CurrentUserId);


            bool task = await basketRepository.AddToBasket(currentUserId, CargoId,cancellationToken);
            if (task == false)
            {
                return Content("مشکلی رخ داده");
            }
            return Ok();
        }

        [PermissionAuthorize(Permissions.Basket.ShowOrder)]

        [HttpGet]
        public async Task<List<ShowListOrderDto>> ShowOrder(CancellationToken cancellationToken)
        {
            string CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int currentUserId = Convert.ToInt32(CurrentUserId);

            //var order = basketRepository.ShowOrder(currentUserId ,cancellationToken);

            Order order1 = basketRepository.TableNoTracking.SingleOrDefault(o => o.userId == currentUserId && !o.IsFinaly);

            List<ShowListOrderDto> OrderList = new List<ShowListOrderDto>();

            if (order1 != null)
            {
                var orderDetails = await basketRepository.GetOrderDetails(order1.Id,cancellationToken);

                foreach (var item in orderDetails)
                {
                    var cargo = cargoRepository.TableNoTracking.SingleOrDefault(o => o.Id == item.Id);
                    OrderList.Add(new ShowListOrderDto()
                    {
                        Count = item.CountCargo,
                        Title = cargo.Name,
                        OrderDetailsId = item.Id,
                        price = item.rating,
                        Sum = item.rating*item.CountCargo,
                    });
                }
            }
            return OrderList;
        }

        [PermissionAuthorize(Permissions.Basket.DeleteFromCart)]
        [HttpDelete]
        public async Task<ActionResult> DeleteFromCart(int OrderDetailId,CancellationToken cancellationToken)
        {
            var ResultDelete = await basketRepository.DeleteFromOrder(OrderDetailId, cancellationToken);

            if (ResultDelete == false)
                return Content("عملیات لغو شد");
            return Content("OrderDetails deleted");
        }

        //[HttpPut]
        ////Put : api/Cart/UpdateOrederDetailInOreder
        //public JsonResult UpdateOrederDetailInOreder(int orderDetailid, string command)
        //{
        //    bool UpdateResult = _cartService.UpdateOrderDetails(orderDetailid, command);
        //    if (UpdateResult == false)
        //    {
        //        return new JsonResult(BadRequest());
        //    }
        //    return new JsonResult(Ok());
        //}


    }
}
