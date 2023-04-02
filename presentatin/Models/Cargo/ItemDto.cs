using Entities.Cargo.ItemValue;

namespace presentation.Models.Cargo
{
    public class ItemDto
    {
        public Value Name { get; set; }
        public decimal Whight { get; set; }
        public decimal Rating { get; set; }
        public int CargoId { get; set; }

    }
}
