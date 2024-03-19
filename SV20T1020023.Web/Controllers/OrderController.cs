using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020023.BusinessLayers;
using SV20T1020023.DomainModels;
using SV20T1020023.Web.Models;

namespace SV20T1020023.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Employee}")]

    public class OrderController : Controller
    {
        private const int ORDER_PAGE_SIZE = 20;
        private const string ORDER_SEARCH = "order_search";

        /// <summary>
        /// Giao diện tìm kiếm và hiển thị kết quả tìm kiếm đơn hàng
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            OrderSearchInput? input = ApplicationContext.GetSessionData<OrderSearchInput>(ORDER_SEARCH);
            if (input == null)
            {
                input = new OrderSearchInput()
                {
                    Page = 1,
                    PageSize = ORDER_PAGE_SIZE,
                    SearchValue = "",
                    Status = 0,
                    DateRange = string.Format("{0:dd/MM/yyyy} - {1:dd/MM/yyyy}", DateTime.Today.AddMonths(-1), DateTime.Today)
                };
            }
            return View(input);
        }
        public IActionResult Search(OrderSearchInput input)
        {
            int rowCount = 0;
            var data = OrderDataService.ListOrders(out rowCount, input.Page, input.PageSize, input.Status, input.FromTime, input.ToTime, input.SearchValue ?? "");
            var model = new OrderSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                Status = input.Status,
                TimeRange = input.DateRange ?? "",
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(ORDER_SEARCH, input);
            return View(model);
        }

        public IActionResult Details(int id = 0)
        {
            var order = OrderDataService.GetOrder(id);
            if (order == null)
                return RedirectToAction("Index");
            var details = OrderDataService.ListOrderDetails(id);
            var model = new OrderDetailModel()
            {
                Order = order,
                Details = details
            };
            return View(model);
        }
        /// <summary>
        /// Chuyển đơn hàng sang trạng thái đã được duyệt
        /// </summary>
        /// <param name="id">Mã đơn hàng</param>
        /// <returns></returns>
        public IActionResult Accept(int id = 0)
        {
            bool result = OrderDataService.AcceptOrder(id);
            if (!result)
                TempData["Message"] = "Không thể duyệt đơn hàng này";
            return RedirectToAction("Details", new { id });
        }

        /// <summary>
        /// Chuyển đơn hàng sang trạng thái đã kết thúc.
        /// </summary>
        /// <param name="id">Mã đơn hàng</param>
        /// <returns></returns>
        public IActionResult Finish(int id = 0)
        {
            bool result = OrderDataService.FinishOrder(id);
            if (!result)
                TempData["Message"] = "Không thể ghi nhận trạng thái kết thúc cho đơn hàng này";
            return RedirectToAction("Details", new { id });
        }

        /// <summary>
        /// Chuyển đơn hàng sang trạng thái bị hủy
        /// </summary>
        /// <param name="id">Mã đơn hàng</param>
        /// <returns></returns>
        public IActionResult Cancel(int id = 0)
        {
            bool result = OrderDataService.CancelOrder(id);
            if (!result)
                TempData["Message"] = "Không thể thực hiện thao tác hủy đối với đơn hàng này";
            return RedirectToAction("Details", new { id });
        }

        /// <summary>
        /// Chuyển đơn hàng sang trạng thái bị từ chối
        /// </summary>
        /// <param name="id">Mã đơn hàng</param>
        /// <returns></returns>
        public IActionResult Reject(int id = 0)
        {
            bool result = OrderDataService.RejectOrder(id);
            if (!result)
                TempData["Message"] = "Không thể thực hiện thao tác từ chối đối với đơn hàng này";
            return RedirectToAction("Details", new { id });
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult EditDetail(int id = 0, int productId = 0)
        {
            return View();
        }
        /// <summary>
        /// Xóa đơn hàng
        /// </summary>
        /// <param name="id">Mã đơn hàng</param>
        /// <returns></returns>
        public IActionResult Delete(int id)
        {
            bool result = OrderDataService.DeleteOrder(id);
            if (!result)
            {
                TempData["Message"] = "Không thể xóa đơn hàng này";
                return RedirectToAction("Details", new { id });
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Giao diện để chọn người giao hàng cho đơn hàng
        /// </summary>
        /// <param name="id">Mã đơn hàng</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Shipping(int id = 0)
        {
            ViewBag.OrderID = id;
            return View();
        }

        /// <summary>
        /// Ghi nhận người giao hàng cho đơn hàng và chuyển đơn hàng sang trạng thái đang giao hàng.
        /// Hàm trả về chuỗi khác rỗng thông báo lỗi nếu đầu vào không hợp lệ hoặc lỗi, 
        /// hàm trả về chuỗi rỗng nếu thành công
        /// </summary>
        /// <param name="id">Mã đơn hàng</param>
        /// <param name="shipperID">Mã người giao hàng</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Shipping(int id = 0, int shipperID = 0)
        {
            if (shipperID <= 0)
                return Json("Vui lòng chọn người giao hàng");

            bool result = OrderDataService.ShipOrder(id, shipperID);
            if (!result)
                return Json("Đơn hàng không cho phép chuyển cho người giao hàng");

            return Json(""); // Trả về chuỗi rỗng nếu không có lỗi
        }
    }
}
