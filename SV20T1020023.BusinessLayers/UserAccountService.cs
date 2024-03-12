using SV20T1020023.DataLayers.SQLServer;
using SV20T1020023.DataLayers;
using SV20T1020023.DomainModels;

namespace SV20T1020023.BusinessLayers
{
    public class UserAccountService
    {
        private static readonly IUserAccountDAL _userAccountDAL;
        static UserAccountService()
        {
            _userAccountDAL = new EmployeeAccountDAL(Configuration.ConnectionString);
        }
        public static UserAccount? Authorize(string userName, string password)
        {
            //TODO: Kiểm tra thông tin đăng nhập của Employee
            return _userAccountDAL.Authorize(userName, password);
        }
        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {

            //TODO: Thay đổi mật khẩu của Employee
            return _userAccountDAL.ChangePassword(userName, oldPassword, newPassword);

        }
    }
}
