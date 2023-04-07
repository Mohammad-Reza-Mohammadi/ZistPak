using Data.Contracts;
using ECommerce.Utility;
using Entities.Cargo;
using Entities.Useres;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utility.SwaggerConfig;
using Utility.Utility;

namespace Data.Repositories
{
    public class CargoRepository : Repository<Cargo>, ICargoRepository
    {
        public CargoRepository(ZPakContext dbContext) : base(dbContext)
        {
        }


        public async Task AddCargoAsync(Cargo cargo, CancellationToken cancellationToken)
        {
            //await DbContext.Set<Cargo>().AddAsync(cargo, cancellationToken);
            //DbContext.SaveChanges();

            cargo.CreateDate = DateTime.Now.ToShamsi();
            await base.AddAsync(cargo, cancellationToken);
            return;
        }



    }
}
