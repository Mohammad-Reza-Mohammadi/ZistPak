using Data.Repositories;
using Entities.Cargo;
using presentation.Models.Cargo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface ICargoRepository:IRepository<Cargo>
    {
        public Task AddCargoAsync(CargoDto cargo, CancellationToken cancellationToken);
        public Task UpdateCargoAsnc(UpdateCargoDto updateCargoDto, CancellationToken cancellationToken);
    }
}
