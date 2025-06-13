namespace WebApi.Filters
{
    public class ProductFilter
    {
        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
