using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Municipality
{
    public class MunicipalityPermissions
    {
        public int Id { get; set; }
        public string Permission { get; set; }

        #region navigation Property
        public int municiaplityId { get; set; }
        public Municipality municipality { get; set; }
        #endregion
    }
}
