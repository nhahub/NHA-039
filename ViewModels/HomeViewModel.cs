namespace Shoppia.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Shoppia.Models.Product> FeaturedProducts { get; set; }
        public IEnumerable<Shoppia.Models.Vendor> TopVendors { get; set; }
    }
}
