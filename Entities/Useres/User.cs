using Entities.BaseEntityFolder;
using Entities.Municipality;
using Entities.User.Enum;
using Entities.User.Owned;
using Entities.User.UserProprety.EnumProperty;
using Entities.Useres.UserProprety.EnumProperty;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Useres
{
    public class User:IntBaseEntity
    {
        public User()
        {   
            IsActive = true;
        }
        public string HashPassword { get; set; }
        public int? Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Token { get; set; }
        public bool IsActive { get; set; }

        #region Enum Property
        public Gender Gender { get; set; }
        public Role Role { get; set; }
        public PermissionLevel permissionLevel { get; set; }
        #endregion

        #region OwnedType Property
        public FullName FullName { get; set; }
        public ICollection<Address> Addresses { get; set; }
        #endregion

        #region Navigation Property
        //Municipality
        public int? municipalityId { get; set; }
        public Municipality.Municipality municipality { get; set; }


        public int? ParetnEmployeeId { get; set; }
        public User ParentEmployee { get; set; }
        public ICollection<User> ChileEmployee { get; set; }
        #endregion
    }
}
