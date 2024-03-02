using CartManagmentSystem.Models;

namespace CartManagmentSystem.Interface
{
    /// <summary>
    /// Interface for managing users.
    /// </summary>
    public interface IUser : IDisposable
    {
        /// <summary>
        /// Retrieves user data.
        /// </summary>
        /// <param name="logId">The ID of the log entry.</param>
        /// <param name="data">Output parameter containing the list of user data.</param>
        /// <param name="savedMessage">Output parameter containing the status message.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        bool GetUsers(int logId, out List<UserData> data, out string savedMessage);
    }
}
