namespace presentation.Models
{
    public class ShowListOrderDto
    {
        public string Title { get; set; }
        public int Count { get; set; }
        public decimal Star { get; set; }
        public decimal Sum { get; set; }
        public int OrderDetailsId { get; set; }
    }
}
