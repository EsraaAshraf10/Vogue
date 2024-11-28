namespace Vogue.Dashboard.Models
{
    public class ProductListViewModel
    {
        public IReadOnlyList<ProductViewModel> Products { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalProducts { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalProducts / PageSize);
    }
}
