using SV20T1020023.DomainModels;

namespace SV20T1020023.DataLayers
{
    public interface IUserAccountDAL
    {
        UserAccount? Authorize(string userName, string password);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }
}
