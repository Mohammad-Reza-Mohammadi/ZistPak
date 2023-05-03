using Data.Repositories;
using Entities.User.UserProprety.EnumProperty;
using Entities.Useres;
using Microsoft.AspNetCore.Http;
using presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface IUserRepository:IRepository<User>
    {
        public Task<bool> AddUserAsync(SignupUserDto signupUserDto, CancellationToken cancellationToken);
        public Task UpdateUserAsync(User user, int UserId, IFormFile formFile, CancellationToken cancellationToken);
        public Task<bool> ChangePermissinByID(int id, CancellationToken cancellationToken);
        public Task AllSupervisorChangePermissin(CancellationToken cancellationToken);
        public Task<bool> GetUserByName(string userName);
        public Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
        public Task FinalizeThePurchase(int userId, CancellationToken cancellationToken);



    }
}
