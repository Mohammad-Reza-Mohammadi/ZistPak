using Data.Repositories;
using Entities.Cargo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface ICargoRepository:IRepository<Cargo>
    {
        public Task AddCargoAsync(Cargo cargo, CancellationToken cancellationToken);
    }
}
