using Entities.BaseEntityFolder;
using Entities.Cargo.ItemValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Cargo
{
    public class Item:IntBaseEntity
    {
        public Value ItemValue { get; set; }
        public decimal ItemWhight { get; set; }
        public decimal ItemStar { get; set; }


        #region Relational Property
        public int CargoId { get; set; }
        public Cargo Cargo { get; set; }
        #endregion


    }
}
