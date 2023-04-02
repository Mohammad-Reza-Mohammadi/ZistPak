using Data.Repositories;
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
        public Task AddAsync(User user, string password, CancellationToken cancellationToken);
        public Task UpdateUserAsync(User user, int UserId, IFormFile formFile, CancellationToken cancellationToken);
        public Task<User> Login(string firstName, string password, CancellationToken cancellationToken);



    }
}
