using Data.Contracts;
using ECommerce.Utility;
using Entities.Cargo;
using Entities.Cargo.CargoStatus;
using Entities.ModelsDto.Cargo;
using Microsoft.EntityFrameworkCore;
using presentation.Models.Cargo;
using Utility.Exceptions;

namespace Data.Repositories
{
    public class CargoRepository : Repository<Cargo>, ICargoRepository
    {
        public CargoRepository(ZPakContext dbContext) : base(dbContext)
        {
        }


        public async Task<ResponseOfCreatCargo> AddCargoAsync(CargoDto cargoDto, CancellationToken cancellationToken)
        {
            Cargo cargo = new Cargo()
            {
                CargoName = cargoDto.Name,
                CargoStatus = Status.Imported,
                CargoWhight = 0,
                CargoStar = 0,
                ItemCount = 0,
            };
            cargo.CreateDate = DateTime.Now.ToShamsi();

            await base.AddAsync(cargo, cancellationToken);

            var responseOfCreatCargo = new ResponseOfCreatCargo()
            {
                cagoId = cargo.Id,
                cargoName = cargo.CargoName
            };
            return responseOfCreatCargo;
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
        public async Task<List<GetCargo>> GetAllConfrimCargot( CancellationToken cancellationToken)
        {
            var cargos = await DbContext.Set<Cargo>().ToListAsync(cancellationToken);
            if (cargos == null)
                throw new NotFoundException("محموله ای یافت نشد");

            var confrimCagos = new List<GetCargo>();
            foreach (var cargo in cargos)
            {
                if (cargo.CargoStatus.Equals(Status.Confirm))
                {
                    confrimCagos.Add(new GetCargo
                    {
                        Id = cargo.Id,
                        CargoName = cargo.CargoName,
                        CargoStatus = cargo.CargoStatus,
                        CargoStar = cargo.CargoStar,
                        ItemCount = cargo.ItemCount,
                        CargoWhight = cargo.CargoWhight
                    });
                }
            };
            return  confrimCagos;
        }
        public async Task<GetCargo> GetCargoById(int cargoId,CancellationToken cancellationToken)
        {
            var cargo = await base.GetByIdAsync(cancellationToken, cargoId);
            if (cargo == null)
                throw new NotFoundException("محموله ای یافت نشد");
            if (!cargo.CargoStatus.Equals(Status.Confirm))
                throw new NotFoundException("محموله ای یافت نشد");

            var getCargo = new GetCargo();
            getCargo.Id = cargo.Id;
            getCargo.CargoName = cargo.CargoName;
            getCargo.CargoStar = cargo.CargoStar;
            getCargo.CargoStatus = cargo.CargoStatus;
            getCargo.CargoWhight = getCargo.CargoWhight;

            return getCargo;
        }

    }
}
