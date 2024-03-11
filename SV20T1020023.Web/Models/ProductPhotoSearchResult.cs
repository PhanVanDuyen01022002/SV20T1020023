using SV20T1020023.DomainModels;

namespace SV20T1020023.Web.Models
{
    public class ProductPhotoSearchResult : BasePaginationResult
    {
        public List<ProductPhoto> Data { get; set; }
    }
}
