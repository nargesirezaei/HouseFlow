using HouseFlowPart1.Interfaces;

namespace HouseFlowPart1.Services
{
    public class LogService : ILogService
    {
        public LogService()
        {
            // Create a "Logs" directory if it doesn't exist
            if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");
        }
        // Write a visit log entry asynchronously
        public void WriteVisitAsync(string request)
        {
            WriteSysLogAsync(request, "200");
        }
        // Write an exception log entry asynchronously
        public void WriteExceptionAsync(string request, string requestCode)
        {
            WriteSysLogAsync(request, requestCode);
        }
        // Internal method to write a log entry
        private static void WriteSysLogAsync( string request, string requestCode)
        {
            try
            {
                var path = $"Logs/app_{DateTime.Now:yyyyMMdd_HHMM}.log";
                
                // Create a log file if it doesn't exist
                if (!File.Exists(path))
                {
                    using var file = File.CreateText(path);
                    file.Close();
                }
                // Append the log entry to the file
                File.AppendAllText(path, $"{requestCode}:{request}\n");

            }
            catch (Exception err)
            {
                // Handle log writing errors by throwing an exception
                throw new Exception(err.Message);
            }
}
    }
}
