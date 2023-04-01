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
        public Value Name { get; set; }
        public decimal Whight { get; set; }
        public decimal Rating { get; set; }


        #region Relational Property
        public int CargoId { get; set; }
        public Cargo cargo { get; set; }
        #endregion


    }
}
