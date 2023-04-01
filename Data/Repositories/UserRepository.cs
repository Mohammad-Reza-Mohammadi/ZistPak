using Data.Contracts;
using Entities.Useres;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Utility;

namespace Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ZPakContext _context;
        public UserRepository(ZPakContext dbContext)
            : base(dbContext)
        {
        }

        public  Task AddAsync(User user, string password, CancellationToken cancellationToken)
        {
            var passswordHash = SecurityHelper.GetSha256Hash(password);
            user.HashPassword = passswordHash;
            return base.AddAsync(user, cancellationToken);
        }

        public Task UpdateUserAsync(User user,int userId,IFormFile userImageFile, CancellationToken cancellationToken)
        {
            string ImagPath = UserImageExtension.ImgeToString(userImageFile);
            user.Image = ImagPath;
            return base.UpdateAsync(user, cancellationToken);
            
        }
    }
}
