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

        [AllowAnonymous]
        [HttpGet]
        public async Task<ApiResult<List<Item>>> GetAllItem(CancellationToken cancellationToken)
        {
            var items = await _itemRepository.TableNoTracking.ToListAsync(cancellationToken);
            if (items.Count == 0)
                return Content("آیتمی یافت نشد");
            return items;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ApiResult<Item>> GetItemById(int id, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetByIdAsync(cancellationToken, id);
            if (item == null)
                return Content("آیتم یافت نشد");
            return item;
        }

        //[PermissionAuthorize(Permissions.Item.AddItem, Admin.admin)]
        [HttpPost]
        public async Task<ApiResult> AddItem([FromForm] AddItemDto addItemDto, CancellationToken cancellationToken)
        {
            int cargoId = addItemDto.CargoId;
            
            var cargo1 = await _cargoRepository.GetByIdAsync(cancellationToken, cargoId);    

            if(cargo1 == null)
            {
                return Content("محموله یافت نشد");
            }
            var item = new Item()
            {
                CreateDate = DateTime.Now.ToShamsi(),
                ItemStar = addItemDto.Rating,
                ItemValue = addItemDto.Value,
                ItemWhight = addItemDto.Whight,
                CargoId = addItemDto.CargoId,
            };

            await _itemRepository.AddAsync(item, cancellationToken);

            
            List<Item> items2 = await _itemRepository.GetItemByCargoId(cargoId, cancellationToken);
            Cargo cargo = await _cargoRepository.GetByIdAsync(cancellationToken, cargoId);//Update Cargo
            {
                cargo.CargoWhight = items2.Sum(i => i.ItemWhight);// وزن محموله
                cargo.CargoStar = items2.Sum(i => i.ItemStar);// امتیاز محموله
                cargo.ItemCount = items2.Count ;//تعداد ایتم موجود در محموله
                cargo.UpdateDate = DateTime.Now.ToShamsi();// تاریخ اپدیت
            };

            await _cargoRepository.UpdateAsync(cargo, cancellationToken);
            return Ok();
        }

        //[PermissionAuthorize(Permissions.Item.UpdateItem, Admin.admin)]
        [HttpPut]
        public async Task<ApiResult> UpdateItem([FromForm] UpdateItemDto updateItemDto, CancellationToken cancellationToken)
        {
            int itemId = updateItemDto.ItemId;
            int cargoId = updateItemDto.CargoId;

            Item item = await _itemRepository.GetByIdAsync(cancellationToken, itemId);
            if (itemId == null)
            {
                return NotFound();
            }

            item.UpdateDate = DateTime.Now.ToShamsi();
            item.ItemWhight = updateItemDto.Whight;
            item.ItemStar = updateItemDto.Rating;
            item.ItemValue = updateItemDto.Value;
            item.CargoId = updateItemDto.CargoId;

            await _itemRepository.UpdateAsync(item, cancellationToken);

            List<Item> listItem = await _itemRepository.GetItemByCargoId(cargoId, cancellationToken);
            Cargo cargo = await _cargoRepository.GetByIdAsync(cancellationToken, cargoId);//update Cargo
            {
                cargo.CargoWhight = listItem.Sum(i => i.ItemWhight);
                cargo.CargoStar = listItem.Sum(i => i.ItemStar);
                cargo.ItemCount = listItem.Count;
                cargo.UpdateDate = DateTime.Now.ToShamsi();
            };

            await _cargoRepository.UpdateAsync(cargo, cancellationToken);

            return Ok();
        }

        //[PermissionAuthorize(Permissions.Item.Delete, Admin.admin)]
        [HttpDelete("{id:int}")]
        public async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            Item item = await _itemRepository.GetByIdAsync(cancellationToken, id);
            int cargoId = item.CargoId;

            await _itemRepository.DeleteAsync(item, cancellationToken);

            List<Item> listItem = await _itemRepository.GetItemByCargoId(cargoId, cancellationToken);
            Cargo cargo = await _cargoRepository.GetByIdAsync(cancellationToken, cargoId);
            {
                cargo.CargoWhight = listItem.Sum(i => i.ItemWhight);
                cargo.CargoStar = listItem.Sum(i => i.ItemStar);
                cargo.ItemCount = listItem.Count;
                cargo.UpdateDate = DateTime.Now.ToShamsi();
            };

            await _cargoRepository.UpdateAsync(cargo, cancellationToken);

            return Ok();
        }

    }
}
