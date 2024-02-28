using CartManagmentSystem.Models;

namespace CartManagmentSystem.Interface
{
    public interface IUser : IDisposable
    {
        bool GetUsers(int logId, out List<UserData> data, out string savedMessage);
    }
}
