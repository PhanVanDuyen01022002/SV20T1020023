using SV20T1020023.DomainModels;

namespace SV20T1020023.Web.Models
{
    /// <summary>
    /// Kết quả tìm kiếm và lấy danh sách khách hàng
    /// </summary>
    public class CustomerSearchResult : BasePaginationResult
    {
        public List<Customer> Data { get; set; }
    }
}
