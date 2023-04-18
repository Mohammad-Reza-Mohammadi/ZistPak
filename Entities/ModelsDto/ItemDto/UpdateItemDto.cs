using Entities.Cargo.ItemValue;

namespace presentation.Models.ItemDto
{
    public class UpdateItemDto
    {
        public int ItemId { get; set; }
        public int CargoId { get; set; }
        public Value Value { get; set; }
        public decimal Whight { get; set; }
        public decimal Rating { get; set; }
    }
}
