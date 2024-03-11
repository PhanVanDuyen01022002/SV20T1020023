using SV20T1020023.DomainModels;
namespace SV20T1020023.Web.Models
{
    /// <summary>
    /// Kết quả tìm kiếm và lấy danh sách nhân viên
    /// </summary>
    public class EmployeeSearchResult : BasePaginationResult
    {
        public List<Employee> Data { get; set; }
    }
}
