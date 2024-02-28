using CartManagmentSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartManagmentSystem.Models
{
    public static class UserFunctions
    {
        public static bool GetAuthorizationDetails(string token, string Key, out string name, out string Id)
        {
            bool worked = false;
            name = string.Empty;
            Id = "0";
            try
            {
                using (CartManagementSystemContext db = new ())
                {

                    Application auth = db.Applications.FirstOrDefault(x => x.SourceToken == token && x.SourceKey == Key && x.Status == true);
                    if (auth != null)
                    {
                        worked = true;
                        name = auth.Name;
                        Id = auth.Id.ToString();
                    }
                }
            }
            catch (Exception)
            {

            }
            return worked;
        }

        public static int InsertLog(string userAgent, string userFunction, string sourceName, string message)
        {
            int id = 0;
            try
            {
                using (CartManagementSystemContext db = new ())
                {

                    UserLog userLog = new UserLog
                    {
                        UserAgent = userAgent,
                        UserFunction = userFunction,
                        SourceName = sourceName,
                        StartDate = DateTime.UtcNow,
                        RequestBody = message
                    };
                    db.UserLogs.Add(userLog);
                    db.SaveChanges();
                    id = userLog.Id;

                }
            }
            catch (Exception ex)
            {
                _ = ex.ToString();
            }
            return id;
        }

        public static void UpdateLogs(int id, int transStatus, int sourceID, string message, string sourceName)
        {
            try
            {
                using (CartManagementSystemContext db = new ())
                {
                    var userLog = db.UserLogs.Find(id);
                    userLog.TransStatus = transStatus;
                    userLog.SourceId = sourceID;
                    userLog.ResponseBody = message;
                    userLog.EndDate = DateTime.UtcNow;
                    userLog.SourceName = sourceName;
                    db.Entry(userLog).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }
            catch (Exception)
            {


            }

        }

        public static void WriteLog(int logId, string request, string response, string serviceName, string callerName)
        {
            //string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Content\\Logs\\" + serviceName + "\\");
            //logFilePath = logFilePath + "Log-" + DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";

            string logFilePath = "C:\\Logs\\" + serviceName + "\\";
            logFilePath = logFilePath + "Log-" + DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";

            try
            {
                using (FileStream fileStream = new FileStream(logFilePath, FileMode.Append))
                {
                    FileInfo logFileInfo;

                    logFileInfo = new FileInfo(logFilePath);
                    DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                    if (!logDirInfo.Exists) logDirInfo.Create();

                    StreamWriter log = new(fileStream);

                    if (!logFileInfo.Exists)
                    {
                        _ = logFileInfo.Create();
                    }
                    else
                    {
                        log.WriteLine(logId);
                        log.WriteLine(DateTime.UtcNow.ToString());
                        log.WriteLine(request);
                        log.WriteLine(response);
                        log.WriteLine(callerName);
                        log.WriteLine("_________________________________________________");
                        log.Close();
                    }
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

    }
}
