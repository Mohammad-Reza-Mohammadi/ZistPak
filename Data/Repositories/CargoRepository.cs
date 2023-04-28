using Data.Contracts;
using ECommerce.Utility;
using Entities.Cargo;
using Entities.Useres;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using presentation.Models.Cargo;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utility.Exceptions;
using Utility.SwaggerConfig;
using Utility.Utility;

namespace Data.Repositories
{
    public class CargoRepository : Repository<Cargo>, ICargoRepository
    {
        public CargoRepository(ZPakContext dbContext) : base(dbContext)
        {
        }


        public async Task AddCargoAsync(CargoDto cargoDto, CancellationToken cancellationToken)
        {
            //await DbContext.Set<Cargo>().AddAsync(cargo, cancellationToken);
            //DbContext.SaveChanges();
            Cargo cargo = new Cargo()
            {
                CargoName = cargoDto.Name,
                CargoStatus = 0,
                CargoWhight = 0,
                CargoStar = 0,
                ItemCount = 0,
            };
            cargo.CreateDate = DateTime.Now.ToShamsi();

            await base.AddAsync(cargo, cancellationToken);
            //return;
        }
        public async Task UpdateCargoAsnc(UpdateCargoDto updateCargoDto, CancellationToken cancellationToken)
        {
            int cargoId = updateCargoDto.cargoId;

            Cargo cargo = await base.GetByIdAsync(cancellationToken, cargoId);
            if (cargo == null)
            {
                throw new NotFoundException("محموله مورد نظر یافت نشد");
            }
            cargo.UpdateDate = DateTime.Now.ToShamsi();
            cargo.CargoName = updateCargoDto.Name;
            cargo.CargoStatus = updateCargoDto.status;
            //تغییرات وزن و تعداد آیتم ها و امتیاز محموله با آپدیت کردن آیتم های محموله انجام میشود

            await base.UpdateAsync(cargo, cancellationToken);

        }




    }
}
