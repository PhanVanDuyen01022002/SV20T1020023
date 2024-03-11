using SV20T1020023.DomainModels;

namespace SV20T1020023.Web.Models
{
    public class CategorySearchResult : BasePaginationResult
    {
        public List<Category> Data { get; set; }
    }
}
