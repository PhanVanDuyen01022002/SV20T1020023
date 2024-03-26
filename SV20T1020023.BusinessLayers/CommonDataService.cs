using SV20T1020023.DataLayers;
using SV20T1020023.DataLayers.SQLServer;
using SV20T1020023.DomainModels;

namespace SV20T1020023.BusinessLayers
{
    /// <summary>
    /// Cung cấp các chức năng xử lý dữ liệu chung
    /// (tỉnh/thành, khách hàng, nhà cung cấp, loại hàng, người giao hàng, nhân viên)
    /// </summary>
    public static class CommonDataService
    {
        private static readonly ICommonDAL<Province> provinceDB;
        private static readonly ICommonDAL<Supplier> supplierDB;
        private static readonly ICommonDAL<Customer> customerDB;
        private static readonly ICommonDAL<Shipper> shipperDB;
        private static readonly ICommonDAL<Employee> employeeDB;
        private static readonly ICommonDAL<Category> categoryDB;
        static CommonDataService()
        {
            string connectionString = Configuration.ConnectionString;

            provinceDB = new ProvinceDAL(connectionString);
            supplierDB = new SupplierDAL(connectionString);
            customerDB = new CustomerDAL(connectionString);
            shipperDB = new ShipperDAL(connectionString);
            employeeDB = new EmployeeDAL(connectionString);
            categoryDB = new CategoryDAL(connectionString);
        }
        /// <summary>
        /// Danh sach cac tinh/ thanh
        /// </summary>
        /// <returns></returns>
        public static List<Province> ListOfProvinces()
        {
            return provinceDB.List().ToList();
        }

        public static List<Supplier> ListOfSuppliers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize, searchValue).ToList();

        }
        public static List<Supplier> ListOfSuppliers(string searchValue)
        {
            return supplierDB.List(1, 0, searchValue).ToList();
        }

        /// <summary>
        /// Lấy thông tin của 1 Nhà cung cấp theo mã Nhà cung cấp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Supplier? GetSupplier(int id)
        {
            return supplierDB.Get(id);
        }
        /// <summary>
        /// Bổ sung Nhà cung cấp mới
        /// </summary>
        /// <param name="Supplier"></param>
        /// <returns></returns>
        public static int AddSupplier(Supplier Supplier)
        {
            return supplierDB.Add(Supplier);
        }
        /// <summary>
        /// Cập nhật Nhà cung cấp
        /// </summary>
        /// <param name="Supplier"></param>
        /// <returns></returns>
        public static bool UpdateSupplier(Supplier Supplier)
        {
            return supplierDB.Update(Supplier);
        }
        /// <summary>
        /// Xóa Nhà cung cấp có mã là id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteSupplier(int id)
        {
            if (supplierDB.IsUsed(id))
            {
                return false;
            }
            return supplierDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra xem Nhà cung cấp có mã id hiện có liên quan hay không.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedSupplier(int id)
        {
            return supplierDB.IsUsed(id);
        }

        /// <summary>
        /// Tìm kiếm và lấy danh sách khách hàng
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue).ToList();
        }
        public static List<Customer> ListOfCustomers()
        {
            return customerDB.List().ToList();
        }
        /// <summary>
        /// Lấy thông tin của 1 khách hàng theo mã khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Customer? GetCustomer(int id)
        {
            return customerDB.Get(id);
        }
        /// <summary>
        /// Bổ sung khách hàng mới
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static int AddCustomer(Customer customer)
        {
            return customerDB.Add(customer);
        }
        /// <summary>
        /// Cập nhật khách hàng
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer customer)
        {
            return customerDB.Update(customer);
        }
        /// <summary>
        /// Xóa khách hàng có mã là id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCustomer(int id)
        {
            if(customerDB.IsUsed(id))
            {
                return false;
            }
            return customerDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra xem khách hàng có mã id hiện có liên quan hay không.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedCustomer(int id)
        {
            return customerDB.IsUsed(id);
        }

        public static List<Shipper> ListOfShippers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue).ToList();

        }
        public static List<Shipper> ListOfShippers()
        {
            return shipperDB.List().ToList();

        }
        /// <summary>
        /// Lấy thông tin của 1 Shipper theo mã Shipper
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Shipper? GetShipper(int id)
        {
            return shipperDB.Get(id);
        }
        /// <summary>
        /// Bổ sung Shipper mới
        /// </summary>
        /// <param name="Shipper"></param>
        /// <returns></returns>
        public static int AddShipper(Shipper Shipper)
        {
            return shipperDB.Add(Shipper);
        }
        /// <summary>
        /// Cập nhật Shipper
        /// </summary>
        /// <param name="Shipper"></param>
        /// <returns></returns>
        public static bool UpdateShipper(Shipper Shipper)
        {
            return shipperDB.Update(Shipper);
        }
        /// <summary>
        /// Xóa Shipper có mã là id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteShipper(int id)
        {
            if (shipperDB.IsUsed(id))
            {
                return false;
            }
            return shipperDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra xem Shipper có mã id hiện có liên quan hay không.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedShipper(int id)
        {
            return shipperDB.IsUsed(id);
        }

        public static List<Employee> ListOfEmployees(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue).ToList();

        }
        /// <summary>
        /// Lấy thông tin của 1 Nhân viên theo mã Nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Employee? GetEmployee(int id)
        {
            return employeeDB.Get(id);
        }
        /// <summary>
        /// Bổ sung Nhân viên mới
        /// </summary>
        /// <param name="Employee"></param>
        /// <returns></returns>
        public static int AddEmployee(Employee Employee)
        {
            return employeeDB.Add(Employee);
        }
        /// <summary>
        /// Cập nhật Nhân viên
        /// </summary>
        /// <param name="Employee"></param>
        /// <returns></returns>
        public static bool UpdateEmployee(Employee Employee)
        {
            return employeeDB.Update(Employee);
        }
        /// <summary>
        /// Xóa Nhân viên có mã là id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteEmployee(int id)
        {
            if (employeeDB.IsUsed(id))
            {
                return false;
            }
            return employeeDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra xem Nhân viên có mã id hiện có liên quan hay không.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedEmployee(int id)
        {
            return employeeDB.IsUsed(id);
        }

        public static List<Category> ListOfCategorys(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue).ToList();

        }

        public static List<Category> ListOfCategorys(string searchValue)
        {
            return categoryDB.List(1, 0, searchValue).ToList();

        }
        /// <summary>
        /// Lấy thông tin của 1 Loại hàng theo mã Loại hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Category? GetCategory(int id)
        {
            return categoryDB.Get(id);
        }
        /// <summary>
        /// Bổ sung Loại hàng mới
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        public static int AddCategory(Category Category)
        {
            return categoryDB.Add(Category);
        }
        /// <summary>
        /// Cập nhật Loại hàng
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        public static bool UpdateCategory(Category Category)
        {
            return categoryDB.Update(Category);
        }
        /// <summary>
        /// Xóa Loại hàng có mã là id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCategory(int id)
        {
            if (categoryDB.IsUsed(id))
            {
                return false;
            }
            return categoryDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra xem Loại hàng có mã id hiện có liên quan hay không.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedCategory(int id)
        {
            return categoryDB.IsUsed(id);
        }
    }
}
