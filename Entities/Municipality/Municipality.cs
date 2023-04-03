using Entities.BaseEntityFolder;
using Entities.Municipality.Enum;
using Entities.User;
using Entities.Useres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Municipality
{
    public class Municipality:IntBaseEntity
    {
        public string Name { get; set; }
        public string Hashpassword { get; set; }
        public string token { get; set; }
        public Region Region { get; set; }

        #region Navigation Property
        public ICollection<Useres.User> users { get; set; }
        public ICollection<UPermissions> UPermissions { get; set; }
        #endregion
    }
}
