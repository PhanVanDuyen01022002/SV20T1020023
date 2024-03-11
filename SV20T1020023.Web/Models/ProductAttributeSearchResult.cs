using SV20T1020023.DomainModels;

namespace SV20T1020023.Web.Models
{
    public class ProductAttributeSearchResult : BasePaginationResult
    {
        public List<ProductAttribute> Data { get; set; }
    }
}
