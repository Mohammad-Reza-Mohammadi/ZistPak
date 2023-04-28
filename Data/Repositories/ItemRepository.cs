using Data.Contracts;
using ECommerce.Utility;
using Entities.Cargo;
using Microsoft.EntityFrameworkCore;
using presentation.Models.ItemDto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility.Exceptions;

namespace Data.Repositories
{
    public class ItemRepository : Repository<Item>,IItemRepository
    {
        public ItemRepository(ZPakContext dbContext) : base(dbContext)
        {
     
        }



        public async Task AddItemAsync(AddItemDto addItemDto, CancellationToken cancellationToken)
        {
            int cargoId = addItemDto.CargoId;

            var existCoargo = await DbContext.Set<Cargo>().Where(u => u.Id == cargoId).SingleOrDefaultAsync();

            if (existCoargo == null)
            {
                throw new NotFoundException("محموله مورد نظر یافت نشد !");
            }
            var item = new Item()
            {
                CreateDate = DateTime.Now.ToShamsi(),
                ItemStar = addItemDto.Rating,
                ItemValue = addItemDto.Value,
                ItemWhight = addItemDto.Whight,
                CargoId = addItemDto.CargoId,
            };

            await base.AddAsync(item, cancellationToken);


            List<Item> items2 = await GetItemByCargoId(cargoId, cancellationToken);
            Cargo cargo = await DbContext.Set<Cargo>().Where(u => u.Id == cargoId).SingleOrDefaultAsync();//Update Cargo
            {
                cargo.CargoWhight = items2.Sum(i => i.ItemWhight);// وزن محموله
                cargo.CargoStar = items2.Sum(i => i.ItemStar);// امتیاز محموله
                cargo.ItemCount = items2.Count;//تعداد ایتم موجود در محموله
                cargo.UpdateDate = DateTime.Now.ToShamsi();// تاریخ اپدیت
            };

            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateItemAsync(UpdateItemDto updateItemDto, CancellationToken cancellationToken)
        {
            int itemId = updateItemDto.ItemId;
            int cargoId = updateItemDto.CargoId;

            Item item = await base.GetByIdAsync(cancellationToken, itemId);
            if (itemId == null)
            {
                throw new NotFoundException("محموله مورد نظر یافت نشد !");
            }

            item.UpdateDate = DateTime.Now.ToShamsi();
            item.ItemWhight = updateItemDto.Whight;
            item.ItemStar = updateItemDto.Rating;
            item.ItemValue = updateItemDto.Value;
            item.CargoId = updateItemDto.CargoId;

            await base.UpdateAsync(item, cancellationToken);

            List<Item> listItem = await GetItemByCargoId(cargoId, cancellationToken);
            Cargo cargo = await DbContext.Set<Cargo>().Where(u => u.Id == cargoId).SingleOrDefaultAsync();//Update Cargo
            {
                cargo.CargoWhight = listItem.Sum(i => i.ItemWhight);
                cargo.CargoStar = listItem.Sum(i => i.ItemStar);
                cargo.ItemCount = listItem.Count;
                cargo.UpdateDate = DateTime.Now.ToShamsi();
            };
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteItemAsync(int id, CancellationToken cancellationToken)
        {
            Item item = await base.GetByIdAsync(cancellationToken, id);
            int cargoId = item.CargoId;

            await base.DeleteAsync(item, cancellationToken);

            List<Item> listItem = await GetItemByCargoId(cargoId, cancellationToken);
            Cargo cargo = await DbContext.Set<Cargo>().Where(u => u.Id == cargoId).SingleOrDefaultAsync();//Update Cargo
            {
                cargo.CargoWhight = listItem.Sum(i => i.ItemWhight);
                cargo.CargoStar = listItem.Sum(i => i.ItemStar);
                cargo.ItemCount = listItem.Count;
                cargo.UpdateDate = DateTime.Now.ToShamsi();
            };
            await DbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<List<Item>> GetItemByCargoId(int CargoId, CancellationToken cancellationToken)
        {
            //var items = DbContext.Set<Item>().Where(u => u.CargoId == CargoId).ToList();
            var items = await Table.Where(u => u.CargoId == CargoId).ToListAsync();
            return items;
        }
    }
}
