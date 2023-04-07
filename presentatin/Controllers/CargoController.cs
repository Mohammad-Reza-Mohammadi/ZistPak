using Data.Contracts;
using Data.Repositories;
using ECommerce.Utility;
using Entities.Cargo;
using Entities.Cargo.CargoStatus;
using Entities.Useres;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using presentation.Models.Cargo;
using System.Security.Cryptography.Xml;
using Utility.SwaggerConfig.Permissions;
using static Utility.SwaggerConfig.Permissions.Permissions;
using Cargo = Entities.Cargo.Cargo;

namespace presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CargoController : ControllerBase
    {
        private readonly ICargoRepository _cargoRepository;

        public CargoController(ICargoRepository cargoRepository)
        {
            this._cargoRepository = cargoRepository;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<Cargo>>> GetAllCargo(CancellationToken cancellationToken)
        {
            List<Cargo> cargos = await _cargoRepository.TableNoTracking.ToListAsync(cancellationToken);
            if (cargos == null)
                return Content("محموله ای یافت نشد");
            return cargos;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cargo>> GetCargoById(int id, CancellationToken cancellationToken)
        {
            Cargo cargo = await _cargoRepository.GetByIdAsync(cancellationToken, id);
            if (cargo == null)
                return Content("محموله یافت نشد");
            return cargo;
        }

        [PermissionAuthorize(Permissions.Cargo.AddCargo, Admin.admin)]
        [HttpPost]
        public async Task<ActionResult> AddCargo(CargoDto cargoDto, CancellationToken cancellationToken)
        {

            Cargo cargo = new Cargo()
            {
                CargoName = cargoDto.Name,
                CargoStatus = 0,
                CargoWhight = 0,
                CargoStar = 0,
                ItemCount = 0,
            };
            await _cargoRepository.AddCargoAsync(cargo, cancellationToken);
            return Content($"{cargoDto.Name} با موفقیت اضافه شد");
            #region Fk bug
            //int cargoId = itemDtos.First().CargoId;

            //var items = new List<Item>();
            //foreach (var itemDto in itemDtos)
            //{
            //    var item = new Item
            //    {
            //        CreateDate = DateTime.Now.ToShamsi(),
            //        Rating = itemDto.Rating,
            //        Name = itemDto.Name,
            //        Whight = itemDto.Whight,
            //        CargoId = itemDto.CargoId,
            //    };
            //    items.Add(item);
            //}

            //await itemRepository.AddItemAsync(items, cancellationToken);
            //List<Item> items2 = await itemRepository.GetItemByCargoId(cargoId, cancellationToken);

            //Cargo cargo = new Cargo()
            //{
            //    status = status,
            //    Whight = items2.Sum(i => i.Whight),
            //    Rating = items2.Sum(i => i.Rating),
            //    Count = items2.Count,
            //    Items = items2
            //};

            //cargoRepository.AddCargoAsync(cargo, cancellationToken);

            //return Ok();
            #endregion
        }

        [PermissionAuthorize(Permissions.Cargo.UpdateCargo, Admin.admin)]
        [HttpPut]
        public async Task<ActionResult> UpdateCargo(UpdateCargoDto updateCargoDto, CancellationToken cancellationToken)
        {
            int cargoId = updateCargoDto.cargoId;

            Cargo cargo = await _cargoRepository.GetByIdAsync(cancellationToken, cargoId);
            if (cargo == null)
            {
                return NotFound();
            }

            cargo.UpdateDate = DateTime.Now.ToShamsi();
            cargo.CargoName = updateCargoDto.Name;
            cargo.CargoStatus = updateCargoDto.status;
            //تغییرات وزن و تعداد آیتم ها و امتیازم محموله با آپدیت کردن آیتم های محموله انجام میشود

            await _cargoRepository.UpdateAsync(cargo, cancellationToken);

            return Content("محموله با موفقیت به روز رسانی شد");
        }

        [PermissionAuthorize(Permissions.Cargo.DeleteCargo, Admin.admin)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCargo(int id, CancellationToken cancellationToken)
        {
            Cargo cargo = await _cargoRepository.GetByIdAsync(cancellationToken, id);
            cargo.CargoStatus = Status.Rejected;
            await _cargoRepository.UpdateAsync(cargo, cancellationToken);

            return Ok();
        }
    }
}

