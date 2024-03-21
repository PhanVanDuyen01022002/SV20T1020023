using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020023.BusinessLayers;
using SV20T1020023.DomainModels;
using SV20T1020023.Web.Models;

namespace SV20T1020023.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]

    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string PRODUCT_SEARCH = "product_search"; //Tên biến dùng dể lưu trong session

        public IActionResult Index()
        {
            //Lấy đầu vào tìm kiếm hiện đang lưu lại trong session
            ProductSearchInput? input = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH);

            //Trường hợp trong session chưa có điều kiện thì tạo ra điều kiện mới
            if (input == null)
            {
                input = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryID = 0,
                    SupplierID = 0
                };
            }

            return View(input);
        }

        public IActionResult Search(ProductSearchInput input)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "", input.CategoryID, input.SupplierID);
            var model = new ProductSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                CategoryID = input.CategoryID,
                SupplierID = input.SupplierID,
                RowCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiện tìm kiếm vào trong session
            ApplicationContext.SetSessionData(PRODUCT_SEARCH, input);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung mặt hàng";
            Product model = new Product()
            {
                ProductID = 0,
                Photo = "noproduct.png",
                IsSelling = true
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
        public IActionResult Save(Product data, IFormFile? uploadPhoto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.ProductName))
                    ModelState.AddModelError(nameof(data.ProductName), "Tên mặt hàng không được bỏ trống");
                if (data.CategoryID == 0)
                    ModelState.AddModelError(nameof(data.CategoryID), "Vui lòng chọn loại hàng");
                if (data.SupplierID == 0)
                    ModelState.AddModelError(nameof(data.SupplierID), "Vui lòng chọn nhà cung cấp");
                if (string.IsNullOrWhiteSpace(data.Unit))
                    ModelState.AddModelError(nameof(data.Unit), "Đơn vị tính không được bỏ trống");
                if ((data.Price <= 0))
                    ModelState.AddModelError(nameof(data.Price), "Giá hàng không hợp lệ");

                if (!ModelState.IsValid)
                {
                    ViewBag.Tittle = data.ProductID == 0 ? "Bổ sung khách hàng" : "Cập nhật thông tin khách hàng";
                    return View("Edit", data);
                }
                if (uploadPhoto != null)
                {
                    string filename = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                    string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\products");
                    string filepath = Path.Combine(folder, filename);
                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = filename;
                }
                if (data.ProductID == 0)
                {

                    int id = ProductDataService.AddProduct(data);
                    if (id <= 0)
                    {
                        ModelState.AddModelError(nameof(data.ProductName), "Tên mặt hàng bị trùng");
                        return View("Edit", data);
                    }
                    //return Json(data);
                }
                else
                {

                    bool result = ProductDataService.UpdateProduct(data);
                    if (!result)
                    {
                        ModelState.AddModelError(nameof(data.ProductName), "Tên mặt hàng bị trùng");
                        return View("Edit", data);
                    }
                    //return Json(data);
                }
                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "không lưu được dữ liệu vui lòng thử lại sau");
                return View("Edit", data);
            }
        }

        [HttpPost]
        public IActionResult SavePhoto(ProductPhoto data, IFormFile? uploadPhoto)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(data.Description))
                    ModelState.AddModelError(nameof(data.Description), "Mô tả không được để trống");

                if (data.DisplayOrder <= 0)
                    ModelState.AddModelError(nameof(data.DisplayOrder), "Vui lòng chọn thứ tự hiển thị");
                if (!ModelState.IsValid)
                {
                    ViewBag.Tittle = data.PhotoId == 0 ? "Bổ sung ảnh" : "Thay đổi ảnh";
                    return View("Photo", data);
                }
                if (uploadPhoto != null)
                {

                    string filename = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                    string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\products");
                    string filepath = Path.Combine(folder, filename);
                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = filename;
                }
                if (data.PhotoId == 0)
                {
                    long id = ProductDataService.AddPhoto(data);
                }

                else
                {
                    bool result = ProductDataService.UpdatePhoto(data);
                }
                //return Json(data);
                return RedirectToAction("Edit", new { id = data.ProductId });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
                //ModelState.AddModelError("Error", "không lưu được dữ liệu vui lòng thử lại sau");
                //return View("Edit", data);
            }
        }

        [HttpPost]

        public IActionResult SaveAttribute(ProductAttribute data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.AttributeValue))
                    ModelState.AddModelError(nameof(data.AttributeValue), "giá trị thuộc tính không được để trống");
                if (string.IsNullOrWhiteSpace(data.AttributeName))
                    ModelState.AddModelError(nameof(data.AttributeName), "Tên thuộc tính không được bỏ trống");
                if (data.DisplayOrder <= 0)
                    ModelState.AddModelError(nameof(data.DisplayOrder), "Vui lòng chọn thứ tự hiển thị");
                if (!ModelState.IsValid)
                {
                    ViewBag.Tittle = data.AttributeId == 0 ? "Bổ sung thuộc tính" : "Thay đổi thuộc tính";
                    return View("Attribute", data);
                }
                if (data.AttributeId == 0)
                {
                    long id = ProductDataService.AddAttribute(data);
                }
                else
                {
                    bool result = ProductDataService.UpdateAttribute(data);
                }
                //return Json(data);
                return RedirectToAction("Edit", new { id = data.ProductId });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", "không lưu được dữ liệu vui lòng thử lại sau");
                return View("Edit", data);
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
        public IActionResult Photo(int id, string method, int photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh";
                    ProductPhoto dataC = new ProductPhoto()
                    {
                        ProductId = id,
                        PhotoId = 0,
                        Photo = "noproduct.png"
                    };
                    return View(dataC);
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh";
                    ProductPhoto? dataE = ProductDataService.GetPhoto(photoId);
                    if (dataE == null)
                        return RedirectToAction("Edit");
                    //if (string.IsNullOrEmpty(dataE.Photo))
                    /* dataE.Photo = "noneProduct.png";*/
                    return View(dataE);
                case "delete":
                    //TODO: Xóa ảnh (Xóa trực tiếp, không cần xác nhận)
                    ProductDataService.DeletePhoto(photoId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Edit");
            }
        }
        public IActionResult Attribute(int id, string method, int attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính";
                    ProductAttribute dataC = new ProductAttribute()
                    {
                        ProductId = id,
                        AttributeId = 0
                    };
                    return View(dataC);
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính";
                    ProductAttribute? data = ProductDataService.GetAttribute(attributeId);
                    if (data == null)
                        return RedirectToAction("Edit");
                    return View(data);
                case "delete":
                    //TODO: Xóa ảnh (Xóa trực tiếp, không cần xác nhận)
                    ProductDataService.DeleteAttribute(attributeId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Edit");
            }
        }
    }
}
