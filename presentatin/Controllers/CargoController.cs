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
using WebFramework.Api;
using WebFramework.Filters;
using static Utility.SwaggerConfig.Permissions.Permissions;
using Cargo = Entities.Cargo.Cargo;

namespace presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiResultFilter]
    public class CargoController : ControllerBase
    {
        private readonly ICargoRepository _cargoRepository;

        public CargoController(ICargoRepository cargoRepository)
        {
            this._cargoRepository = cargoRepository;
        }

        /// <summary>
        /// گرفتن تمامی محموله ها
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ApiResult<List<Cargo>>> GetAllCargo(CancellationToken cancellationToken)
        {
            List<Cargo> cargos = await _cargoRepository.TableNoTracking.ToListAsync(cancellationToken);
            if (cargos == null)
                return Content("محموله ای یافت نشد");
            return cargos;
        }

        /// <summary>
        /// گرفتن محموله ها با استفاده از ای دی آن
        /// </summary>
        /// <param name="id">آی دی محموله</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ApiResult<Cargo>> GetCargoById(int id, CancellationToken cancellationToken)
        {
            Cargo cargo = await _cargoRepository.GetByIdAsync(cancellationToken, id);
            if (cargo == null)
                return Content("محموله یافت نشد");
            return cargo;
        }

        /// <summary>
        /// اضافه کردن محموله
        /// </summary>
        /// <param name="cargoDto">مشخصات محموله</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(policy: "AddCargoPolicy")]
        [HttpPost]
        public async Task<ApiResult> AddCargo(CargoDto cargoDto, CancellationToken cancellationToken)
        {
            await _cargoRepository.AddCargoAsync(cargoDto, cancellationToken);

            return Content($"{cargoDto.Name} با موفقیت اضافه شد");
        }

        /// <summary>
        /// آپدیت کردن محموله
        /// </summary>
        /// <param name="updateCargoDto">مشخصات محموله</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(policy: "UpdateCargoPolicy")]
        [HttpPut]
        public async Task<ApiResult> UpdateCargo(UpdateCargoDto updateCargoDto, CancellationToken cancellationToken)
        {
            await _cargoRepository.UpdateCargoAsnc(updateCargoDto, cancellationToken);
            return Content("محموله با موفقیت به روز رسانی شد");
        }

        /// <summary>
        /// حذف محموله با استفاده از آی دی آن
        /// </summary>
        /// <param name="id">آی دی محموله</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(policy: "DeleteCargoPolicy")]
        [HttpDelete("{id:int}")]
        public async Task<ApiResult> DeleteCargo(int id, CancellationToken cancellationToken)
        {
            Cargo cargo = await _cargoRepository.GetByIdAsync(cancellationToken, id);
            cargo.CargoStatus = Status.Rejected;
            await _cargoRepository.UpdateAsync(cargo, cancellationToken);

            return Ok();
        }
    }
}

