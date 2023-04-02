using Data.Contracts;
using Entities.Cargo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ItemRepository : Repository<Item>,IItemRepository
    {
        public ItemRepository(ZPakContext dbContext) : base(dbContext)
        {
     
        }

        public async Task AddItemAsync(IEnumerable<Item> Items, CancellationToken cancellationToken)
        {
            await base.AddRangeAsync(Items, cancellationToken);
            return;
        }

        public async Task<List<Item>> GetItemByCargoId(int CargoId, CancellationToken cancellationToken)
        {
            //var items = DbContext.Set<Item>().Where(u => u.CargoId == CargoId).ToList();
            var items = await Table.Where(u => u.CargoId == CargoId).ToListAsync();
            return items;
        }

    }
}
