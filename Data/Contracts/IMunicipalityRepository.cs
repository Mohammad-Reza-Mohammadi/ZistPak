using Data.Repositories;
using Entities.Municipality;
using Entities.Useres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface IMunicipalityRepository:IRepository<Municipality>
    {
        public Task AddAsync(Municipality municipality, string password, CancellationToken cancellationToken);
        public Task<Municipality> LoginAsync(string name, string password, CancellationToken cancellationToken);
        public Task<bool> ChangePermissinByID(int id, CancellationToken cancellationToken);
        public Task AllSupervisorChangePermissin(CancellationToken cancellationToken);



    }
}
