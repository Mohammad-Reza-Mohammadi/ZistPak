using Entities.Cargo.CargoStatus;

namespace presentation.Models.Cargo
{
    public class UpdateCargoDto
    {
        public int CargoId { get; set; }
        public string Name { get; set; }
        public Status status { get; set; }
        public decimal Rating { get; set; }
        public decimal Whight { get; set; }
    }
}
