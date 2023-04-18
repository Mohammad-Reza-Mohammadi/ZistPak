using Entities.BaseEntityFolder;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Useres
{
    public class Role:IdentityRole<int>,IEntity
    {
        public string Description { get; set; }
    }
}
