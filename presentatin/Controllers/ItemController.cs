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

namespace presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        public IItemRepository itemRepository { get; }
        public ICargoRepository cargoRepository { get; }

        public ItemController(IItemRepository _itemRepository, ICargoRepository _cargoRepository)
        {
            itemRepository = _itemRepository;
            cargoRepository = _cargoRepository;
        }

        [HttpGet]
        public async Task<List<Item>> GetAllItem(CancellationToken cancellationToken)
        {
            var cargos = await itemRepository.TableNoTracking.ToListAsync(cancellationToken);
            return cargos;
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Item>> GetItemById(int id, CancellationToken cancellationToken)
        {
            var item = await itemRepository.GetByIdAsync(cancellationToken, id);
            if (item == null)
                return NotFound();
            return item;
        }

        [PermissionAuthorize(Permissions.Item.AddItem)]
        [HttpPost]
        public async Task<ActionResult> AddItem([FromForm] AddItemDto addItemDto, CancellationToken cancellationToken)
        {
            int cargoId = addItemDto.CargoId;           

                var item = new Item()
                {
                    CreateDate = DateTime.Now.ToShamsi(),
                    Rating = addItemDto.Rating,
                    Name = addItemDto.Name,
                    Whight = addItemDto.Whight,
                    CargoId = addItemDto.CargoId,
                };        
        
            await itemRepository.AddAsync(item, cancellationToken);

            List<Item> items2 = await itemRepository.GetItemByCargoId(cargoId, cancellationToken);
            Cargo cargo = await cargoRepository.GetByIdAsync(cancellationToken, cargoId);
            {
                cargo.Whight = items2.Sum(i => i.Whight);
                cargo.Rating = items2.Sum(i => i.Rating);
                cargo.Count = items2.Count;
                cargo.UpdateDate = DateTime.Now.ToShamsi();
            };

            await cargoRepository.UpdateAsync(cargo, cancellationToken);
            return Ok("Items Successfully Added");
        }


        [PermissionAuthorize(Permissions.Item.UpdateItem)]
        [HttpPut]
        public async Task<ActionResult> UpdateItem([FromForm] UpdateItemDto updateItemDto, CancellationToken cancellationToken)
        {
            int itemId = updateItemDto.ItemId;
            int cargoId = updateItemDto.CargoId;

            Item item = await itemRepository.GetByIdAsync(cancellationToken, itemId);
            if (itemId == null)
            {
                return NotFound();
            }

            item.UpdateDate = DateTime.Now.ToShamsi();
            item.Whight = updateItemDto.Whight;
            item.Rating = updateItemDto.Rating;
            item.Name = updateItemDto.Name;
            item.CargoId = updateItemDto.CargoId;

            await itemRepository.UpdateAsync(item, cancellationToken);

            List<Item> listItem = await itemRepository.GetItemByCargoId(cargoId, cancellationToken);
            Cargo cargo = await cargoRepository.GetByIdAsync(cancellationToken, cargoId);
            {
                cargo.Whight = listItem.Sum(i => i.Whight);
                cargo.Rating = listItem.Sum(i => i.Rating);
                cargo.Count = listItem.Count;
                cargo.UpdateDate = DateTime.Now.ToShamsi();
            };

            await cargoRepository.UpdateAsync(cargo, cancellationToken);

            return Ok();
        }

        [PermissionAuthorize(Permissions.Item.Delete)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var item = await itemRepository.GetByIdAsync(cancellationToken, id);
            var cargoId = item.CargoId;
            await itemRepository.DeleteAsync(item, cancellationToken);

            List<Item> listItem = await itemRepository.GetItemByCargoId(cargoId, cancellationToken);
            Cargo cargo = await cargoRepository.GetByIdAsync(cancellationToken, cargoId);
            {
                cargo.Whight = listItem.Sum(i => i.Whight);
                cargo.Rating = listItem.Sum(i => i.Rating);
                cargo.Count = listItem.Count;
                cargo.UpdateDate = DateTime.Now.ToShamsi();
            };

            await cargoRepository.UpdateAsync(cargo, cancellationToken);

            return Ok();
        }

    }
}
