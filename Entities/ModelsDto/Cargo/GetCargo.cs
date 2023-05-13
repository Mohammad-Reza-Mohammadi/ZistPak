using Entities.Cargo.CargoStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ModelsDto.Cargo
{
    public class GetCargo
    {
        public int Id { get; set; }
        public string CargoName { get; set; }
        public Status CargoStatus { get; set; }
        public decimal CargoStar { get; set; }
        public int ItemCount { get; set; }
        public decimal CargoWhight { get; set; }


    }
}
