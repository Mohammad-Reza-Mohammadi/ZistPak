using Entities.BaseEntityFolder;
//using Entities.Municipality;
//using Entities.Municipality.Enum;
using Entities.User.Enum;
using Entities.User.Owned;
using Entities.User.UserProprety.EnumProperty;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Useres
{
    public class User:IdentityUser<int>,IEntity
    {
        public User()
        {   
            UserIsActive = true;
        }
        public int UserAge { get; set; }
        public string UserImage { get; set; }
        public decimal UserStar { get; set; } = 0;
        public string UserToken { get; set; }
        public bool UserIsActive { get; set; }
        public string CreateDate { get; set; }
        public string? UpdateDate { get; set; }
        public string? LastLoginDate { get; set; }



        #region Enum Property
        public UserGender UserGender { get; set; }
        //public UserRole UserRole { get; set; }
        #endregion

        #region OwnedType Property
        public FullName UserFullName { get; set; }
        public virtual ICollection<Address> UserAddresses { get; set; }
        #endregion

        #region Navigation Property
        ////Permission
        //public virtual ICollection<UPermissions> UserPermissions { get; set; }
        //Employee
        public int? UserParetnEmployeeId { get; set; }
        public User UserParentEmployee { get; set; }
        public virtual ICollection<User> UserChiledEmployee { get; set; }
        #endregion
    }
}
