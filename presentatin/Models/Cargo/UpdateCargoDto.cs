using Entities.Cargo.CargoStatus;

namespace presentation.Models.Cargo
{
    public class UpdateCargoDto
    {
        public int cargoId { get; set; }
        public string Name { get; set; }
        public Status status { get; set; }

    }
}
