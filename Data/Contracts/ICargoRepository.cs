using Data.Repositories;
using Entities.Cargo;
using Entities.ModelsDto.Cargo;
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
        public Task<ResponseOfCreatCargo> AddCargoAsync(CargoDto cargo, CancellationToken cancellationToken);
        public Task UpdateCargoAsnc(UpdateCargoDto updateCargoDto, CancellationToken cancellationToken);
        public Task<List<GetCargo>> GetAllConfrimCargot(CancellationToken cancellationToken);
        public Task<GetCargo> GetCargoById(int cargoId, CancellationToken cancellationToken);

    }
}
