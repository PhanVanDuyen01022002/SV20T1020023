using Microsoft.AspNetCore.Mvc;
using SV20T1020023.BusinessLayers;
using SV20T1020023.DomainModels;

namespace SV20T1020023.Web.Controllers
{
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, string searchValue = "", int categoryID = 0, int supplierID = 0)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, page, PAGE_SIZE, searchValue ?? "", categoryID, supplierID);

            var model = new Models.ProductSearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                CategoryID = categoryID,
                SupplierID = supplierID,
                RowCount = rowCount,
                Data = data
            };

            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung mặt hàng";
            Product model = new Product()
            {
                ProductID = 0
            };
            return View("Edit", model);
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Cập nhật thông tin mặt hàng";
            Product? model = ProductDataService.GetProducts(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Product data, string birthDateInput, IFormFile? uploadPhoto)
        {
            try
            {
                /*if (string.IsNullOrWhiteSpace(data.ProductName))
                    ModelState.AddModelError(nameof(data.ProductName), "Tên không được để trống");
                if (data.CategoryID == 0)
                    ModelState.AddModelError(nameof(data.CategoryID), "Vui lòng chọn loại hàng");
                if (data.SupplierID == 0)
                    ModelState.AddModelError(nameof(data.CategoryID), "Vui lòng chọn nhà cung cấp");
                if (string.IsNullOrWhiteSpace(data.Unit))
                    ModelState.AddModelError(nameof(data.Unit), "đơn vị không được để trống");

                //Thông qua thuộc tính IsValid của ModelState để kiểm tra xem có tồn tại lỗi hay không
                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật thông tin mặt hàng";
                    return View("Edit", data);
                }*/

                if (uploadPhoto != null)
                {
                    string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //Tên file sẽ lưu
                    string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, "images\\products"); //Đường dẫn đến thư mục lưu file 
                    string filePath = Path.Combine(folder, fileName); //đường dẫn đến file cần lưu
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = fileName;
                }

                if (data.ProductID == 0)
                {
                    int id = ProductDataService.AddProduct(data);
                    /*return Json(data);*/
                }
                else
                {
                    bool result = ProductDataService.UpdateProduct(data);

                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Lỗi không thể lưu được dữ liệu. Vui lòng thử lại sau ít phút");
                return View("Edit", data);
                /*return Content(ex.Message);*/
            }
        }
        public IActionResult Delete(int id)
        {
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            var model = ProductDataService.GetProducts(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.AllowDelete = !ProductDataService.IsUsedProduct(id);
            return View(model);
        }
        public IActionResult Photo(string id, string method, int photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh";
                    return View();
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh";
                    return View();
                case "delete":
                    //TODO: Xóa ảnh (Xóa trực tiếp, không cần confirm)
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
        public IActionResult Attribute(string id, string method, int attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính";
                    return View();
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính";
                    return View();
                case "delete":
                    //TODO: Xóa ảnh (Xóa trực tiếp, không cần confirm)
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
    }
}
