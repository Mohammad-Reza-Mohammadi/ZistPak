using Data.Repositories;
using Entities.Cargo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface IItemRepository:IRepository<Item>
    {
        public Task<List<Item>> GetItemByCargoId(int CargoId, CancellationToken cancellationToken);

    }
}
