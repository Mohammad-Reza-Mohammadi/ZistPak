using Entities.BaseEntityFolder;
using Microsoft.AspNetCore.Identity;

namespace Entities.Useres
{
    public class Role:IdentityRole<int>,IEntity
    {
        public string Description { get; set; }
    }
}
