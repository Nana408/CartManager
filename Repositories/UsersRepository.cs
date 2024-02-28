using CartManagmentSystem.Entities;
using CartManagmentSystem.Interface;
using CartManagmentSystem.Models;
using Newtonsoft.Json;
using System.Reflection;

namespace CartManagmentSystem.Repositories
{
    public class UsersRepository : IUser
    {
        private readonly CartManagementSystemContext _context;

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

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public bool GetUsers(int logId,out List<UserData> data, out string savedMessage)
        {
            bool worked = false;
            data = new List<UserData>();
            savedMessage = StaticVariables.FAILEDMESSAGE;
            try
            {
                data = _context.Users.Select(x => new UserData
                {
                    Id = x.UserId,
                    Name = x.Username   ,
                    Email = x.Email,
                    PhoneNumber = x.Phonenumber,

                }).ToList();

                if (data.Any())
                {
                    worked = true;
                    savedMessage = StaticVariables.SUCCESSMESSAGE;
                }
            }
            catch (Exception ex)
            {

                savedMessage = StaticVariables.EXCEPTIONMESSAGE;
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(""), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
            }

            return worked;
        }
    }
}
