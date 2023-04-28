using Data.Repositories;
using Entities.Cargo;
using presentation.Models.ItemDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface IItemRepository:IRepository<Item>
    {
        public Task AddItemAsync(AddItemDto addItemDto, CancellationToken cancellationToken);
        public Task UpdateItemAsync(UpdateItemDto updateItemDto, CancellationToken cancellationToken);
        public Task DeleteItemAsync(int id, CancellationToken cancellationToken);


    }
}
