using SV20T1020023.DomainModels;

namespace SV20T1020023.Web.Models
{
    public class OrderSearchResult : BasePaginationResult
    {
        public int Status { get; set; } = 0;
        public string TimeRange { get; set; } = "";
        public List<Order> Data { get; set; } = new List<Order>();
    }
}
