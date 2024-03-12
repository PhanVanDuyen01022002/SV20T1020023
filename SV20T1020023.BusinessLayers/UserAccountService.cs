using SV20T1020023.DomainModels;
namespace SV20T1020023.BusinessLayers
{
    public class UserAccountService
    {
        public static UserAccount? Authorize(string userName, string password)
        {
            //TODO: Kiểm tra thông tin đăng nhập của Employee
            UserAccount data = null;
            // Thực hiện kiểm tra thông tin đăng nhập của Employee
            // Ví dụ: có thể sử dụng truy vấn SQL hoặc gọi đến một dịch vụ kiểm tra đăng nhập
            return data;
        }

        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            //TODO: Thay đổi mật khẩu của Employee
            bool result = false;
            // Thực hiện thay đổi mật khẩu của Employee
            // Ví dụ: có thể sử dụng truy vấn SQL hoặc gọi đến một dịch vụ thay đổi mật khẩu
            return result;
        }

    }
}
