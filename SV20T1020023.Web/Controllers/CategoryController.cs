using Microsoft.AspNetCore.Mvc;
using SV20T1020023.BusinessLayers;
using SV20T1020023.DomainModels;
using System.Drawing.Printing;

namespace SV20T1020023.Web.Controllers
{
    public class CategoryController : Controller
    {
        private const int PAGE_SIZE = 20;
        //TODO: chua hoan chinh vi chua them o view
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCategorys(out rowCount, page, PAGE_SIZE, searchValue ?? "");

            var model = new Models.CategorySearchResult()
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
            ViewBag.Title = "Bổ sung loại hàng";
            Category model = new Category()
            {
                CategoryID = 0,
            };
            return View("Edit", model);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật loại hàng";
            Category model = CommonDataService.GetCategory(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        [HttpPost]
        public IActionResult Save(Category data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.CategoryName))
                    ModelState.AddModelError(nameof(data.CategoryName), "Tên không được để trống");
                if (string.IsNullOrWhiteSpace(data.Description))
                    ModelState.AddModelError(nameof(data.Description), "Vui lòng nhập mô tả cho loại hàng");
                //Thông qua thuộc tính IsValid của ModelState để kiểm tra xem có tồn tại lỗi hay không
                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.CategoryID == 0 ? "Bổ sung khách hàng" : "Cập nhật thông tin khách hàng";
                    return View("Edit", data);
                }

                if (data.CategoryID == 0)
                {
                    int id = CommonDataService.AddCategory(data);
                    /*return Json(data);*/
                }
                else
                {
                    bool result = CommonDataService.UpdateCategory(data);

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
                CommonDataService.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetCategory(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.AllowDelete = !CommonDataService.IsUsedCategory(id);
            return View(model);
        }
    }
}
