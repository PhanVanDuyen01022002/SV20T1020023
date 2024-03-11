using SV20T1020023.DomainModels;

namespace SV20T1020023.Web.Models
{
    public class ProductSearchResult : BasePaginationResult
    {
        public List<Product> Data { get; set; }
        public int CategoryID { get; internal set; }
        public int SupplierID { get; internal set; }
    }
}
