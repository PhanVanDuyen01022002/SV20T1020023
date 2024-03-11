using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace SV20T1020023.web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Create()
        {
            var model = new Models.Person
            {
                Name = "Phan Văn Duyên",
                BirthDate = new DateTime(25/02/2002),
                Salary = 10.25m
            };
            return View(model);
        }
        public IActionResult Save(Models.Person model, string birthDateInput = "")
        {
            //Chuyển birthDateInput sang giá trị kiểu ngày
            DateTime? dValue = StringToDateTime(birthDateInput);
            if(dValue.HasValue)
            {
                model.BirthDate = dValue.Value;
            }
            return Json(model);
        }

        private DateTime? StringToDateTime(string s, string formats = "d/M/yyyy;d-M-yyyy;d.M.yyyy")
        {
            try
            {
                return DateTime.ParseExact(s, formats.Split(','), CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }
    }
}