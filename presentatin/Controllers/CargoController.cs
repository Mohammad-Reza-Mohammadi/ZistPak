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

namespace presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CargoController : ControllerBase
    {
        private readonly ICargoRepository cargoRepository;

        public CargoController(ICargoRepository _cargoRepository)
        {
            this.cargoRepository = _cargoRepository;
        }


        [Authorize(Roles = "Admin,Supervisor")]
        [HttpGet]
        public async Task<List<Cargo>> Get(CancellationToken cancellationToken)
        {
            var cargos = await cargoRepository.TableNoTracking.ToListAsync(cancellationToken);
            return cargos;
        }

        [Authorize(Roles = "Admin,Supervisor")]
        [HttpPost]
        public async Task<ActionResult> AddCargo(CargoDto cargoDto, CancellationToken cancellationToken)
        {

            Cargo cargo = new Cargo()
            {
                Name = cargoDto.Name,
                status = 0,
                Whight = 0,
                Rating = 0,
                Count = 0,
            };
            await cargoRepository.AddCargoAsync(cargo, cancellationToken);


            return Content($"{cargo.Name} با موفقیت اضافه شد");
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

        [Authorize(Roles = "Admin,Supervisor")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cargo>> GetCargoById(int id, CancellationToken cancellationToken)
        {
            var cargo = await cargoRepository.GetByIdAsync(cancellationToken, id);
            if (cargo == null)
                return NotFound();
            return cargo;
        }
        [Authorize(Roles = "Admin,Supervisor")]
        [HttpPut]
        public async Task<ActionResult> Update(UpdateCargoDto updateCargoDto, CancellationToken cancellationToken)
        {
            int cargoId = updateCargoDto.cargoId;

            Cargo cargo = await cargoRepository.GetByIdAsync(cancellationToken, cargoId);
            if (cargo == null)
            {
                return NotFound();
            }

            cargo.UpdateDate = DateTime.Now.ToShamsi();
            cargo.Name = updateCargoDto.Name;
            cargo.status = updateCargoDto.status;
            //تغییرات وزن و تعداد آیتم ها با آپدیت کردن آیتم های محموله انجام میشود

            await cargoRepository.UpdateAsync(cargo, cancellationToken);

            return Ok();
        }
        [Authorize(Roles = "Admin,Supervisor")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var cargo = await cargoRepository.GetByIdAsync(cancellationToken, id);
            cargo.status = Status.Rejected;
            await cargoRepository.UpdateAsync(cargo, cancellationToken);

            return Ok();
        }
    }
}

