using Data.Repositories;
using Entities.User.UserProprety.EnumProperty;
using Entities.Useres;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface IUserRepository:IRepository<User>
    {
        public Task AddUserAsync(User user, string password, CancellationToken cancellationToken);
        public Task UpdateUserAsync(User user, int UserId, IFormFile formFile, CancellationToken cancellationToken);
        public Task<User> Login(string firstName, string password, CancellationToken cancellationToken);
        public Task<bool> ChangePermissinByID(int id, CancellationToken cancellationToken);
        public Task AllSupervisorChangePermissin(CancellationToken cancellationToken);
        public Task<bool> GetUserByName(string userName);


    }
}
