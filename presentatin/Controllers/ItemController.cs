    using Data.Contracts;
using Data.Repositories;
using ECommerce.Utility;
using Entities.Cargo.CargoStatus;
using Entities.Cargo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using presentation.Models.Cargo;
using Microsoft.EntityFrameworkCore;
using presentation.Models.ItemDto;
using Microsoft.AspNetCore.Authorization;
using Utility.SwaggerConfig.Permissions;
using static Utility.SwaggerConfig.Permissions.Permissions;
using Item = Entities.Cargo.Item;
using Cargo = Entities.Cargo.Cargo;
using WebFramework.Api;
using WebFramework.Filters;

namespace presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiResultFilter]
    public class ItemController : ControllerBase
    {
        private IItemRepository _itemRepository { get; }
        private ICargoRepository _cargoRepository { get; }

        public ItemController(IItemRepository itemRepository, ICargoRepository cargoRepository)
        {
            _itemRepository = itemRepository;
            _cargoRepository = cargoRepository;
        }

        /// <summary>
        /// برگرداندن تمامی آیتم ها
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ApiResult<List<Item>>> GetAllItem(CancellationToken cancellationToken)
        {
            var items = await _itemRepository.TableNoTracking.ToListAsync(cancellationToken);
            if (items.Count == 0)
                return Content("آیتمی یافت نشد");
            return items;
        }

        /// <summary>
        /// برگرداندن آیتم با استفاده از آی دی آن
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ApiResult<Item>> GetItemById(int id, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetByIdAsync(cancellationToken, id);
            if (item == null)
                return Content("آیتم یافت نشد");
            return item;
        }

        /// <summary>
        /// اضافه کردن آیتم
        /// </summary>
        /// <param name="addItemDto">مشخصات آیتم</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(policy: "AddItemPolicy")]
        [HttpPost]
        public async Task<ApiResult> AddItem([FromForm] AddItemDto addItemDto, CancellationToken cancellationToken)
        {
            await _itemRepository.AddItemAsync(addItemDto, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// آپدیت کردن آیتم
        /// </summary>
        /// <param name="updateItemDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(policy: "UpdateItemPolicy")]
        [HttpPut]
        public async Task<ApiResult> UpdateItem([FromForm] UpdateItemDto updateItemDto, CancellationToken cancellationToken)
        {
            await _itemRepository.UpdateItemAsync(updateItemDto, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// حذف آیتم با استفاده ازآی دی آن
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(policy: "DeleteItemPolicy")]
        [HttpDelete("{id:int}")]
        public async Task<ApiResult> DeleteItem(int id, CancellationToken cancellationToken)
        {
            await _itemRepository.DeleteItemAsync(id, cancellationToken);
            return Ok();
        }

    }
}
