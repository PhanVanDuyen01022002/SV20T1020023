using Microsoft.AspNetCore.Mvc;
using SV20T1020023.BusinessLayers;
using SV20T1020023.DomainModels;

namespace SV20T1020023.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfEmployees(out rowCount, page, PAGE_SIZE, searchValue ?? "");

            var model = new Models.EmployeeSearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhân viên";
            Employee model = new Employee()
            {
                EmployeeID = 0,
                BirthDate = new DateTime(1990, 1, 1),
                Photo = "user.png"
            };
            return View("Edit", model);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin nhân viên";
            Employee? model = CommonDataService.GetEmployee(id);
            if (model == null)
                return RedirectToAction("Index");
            if (string.IsNullOrEmpty(model.Photo))
                model.Photo = "user.png";
            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Employee data, string birthDateInput, IFormFile? uploadPhoto)
        {

            /*return Json(data);*/
            try
            {
                //Xử lý ngày sinh
                DateTime? birthDate = birthDateInput.ToDateTime();
                if(birthDate.HasValue)
                    data.BirthDate = birthDate.Value;

                if (string.IsNullOrWhiteSpace(data.FullName))
                    ModelState.AddModelError(nameof(data.FullName), "Tên không được để trống");
                if (string.IsNullOrWhiteSpace(data.Address))
                    ModelState.AddModelError(nameof(data.Address), "Địa chỉ không được để trống");
                if (string.IsNullOrWhiteSpace(data.Phone))
                    ModelState.AddModelError(nameof(data.Phone), "Vui lòng nhập số điện thoại của nhân viên");
                if (string.IsNullOrWhiteSpace(data.Email))
                    ModelState.AddModelError(nameof(data.Email), "Vui lòng nhập Email của nhân viên");
                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật thông tin nhân viên";
                    return View("Edit", data);
                }

                //Xử lý ảnh upload ( nếu có ảnh upload thì lưu ảnh và gán lại tên file ảnh mới cho employee)
                if (uploadPhoto != null)
                {
                    string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //Tên file sẽ lưu
                    string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, "images\\employees"); //Đường dẫn đến thư mục lưu file 
                    string filePath = Path.Combine(folder, fileName); //đường dẫn đến file cần lưu
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = fileName;
                }

                if (data.EmployeeID == 0)
                {
                    int id = CommonDataService.AddEmployee(data);
                    if (id <= 0)
                    {
                        ModelState.AddModelError(nameof(data.Email), "Địa chỉ email bị trùng");
                        ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật thông tin nhân viên";
                        return View("Edit", data);
                    }
                    /* return Json(data);*/
                }
                else
                {
                    bool result = CommonDataService.UpdateEmployee(data);
                    if (!result)
                    {
                        ModelState.AddModelError(nameof(data.Email), "Địa chỉ email bị trùng với nhân viên khác");
                        ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật thông tin nhân viên";
                        return View("Edit", data);
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Lỗi không thể lưu được dữ liệu. Vui lòng thử lại sau ít phút");
                return View("Edit", data);
            }
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetEmployee(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.AllowDelete = !CommonDataService.IsUsedEmployee(id);
            return View(model);
        }
    }
}
