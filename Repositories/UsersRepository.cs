using CartManagmentSystem.Entities;
using CartManagmentSystem.Interface;
using CartManagmentSystem.Models;
using Newtonsoft.Json;
using System.Reflection;

namespace CartManagmentSystem.Repositories
{
    /// <summary>
    /// Repository class for handling user-related operations.
    /// </summary>
    public class UsersRepository : IUser
    {
        private readonly CartManagementSystemContext _context;

        /// <summary>
        /// Constructor for UsersRepository class.
        /// </summary>
        /// <param name="context">Instance of CartManagementSystemContext for database operations.</param>
        public UsersRepository(CartManagementSystemContext context)
        {
            _context = context;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Disposes resources used by the repository class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Retrieves users from the database.
        /// </summary>
        /// <param name="logId">Log identifier for tracking errors.</param>
        /// <param name="data">List of UserData representing retrieved users.</param>
        /// <param name="savedMessage">Message indicating the outcome of the operation.</param>
        /// <returns>Boolean indicating whether the operation was successful.</returns>
        public bool GetUsers(int logId, out List<UserData> data, out string savedMessage)
        {
            bool worked = false;
            data = new List<UserData>();
            savedMessage = StaticVariables.FAILEDMESSAGE;
            try
            {
                // Retrieve user data from the database and map it to UserData objects.
                data = _context.Users.Select(x => new UserData
                {
                    Id = x.UserId,
                    Name = x.Username,
                    Email = x.Email,
                    PhoneNumber = x.Phonenumber
                }).ToList();

                // Check if any users were retrieved.
                if (data.Any())
                {
                    worked = true;
                    savedMessage = StaticVariables.SUCCESSMESSAGE;
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the operation.
                savedMessage = StaticVariables.EXCEPTIONMESSAGE;
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(""), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
            }

            return worked;
        }
    }
}