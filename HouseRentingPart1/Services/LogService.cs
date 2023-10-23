using HouseFlowPart1.Interfaces;

namespace HouseFlowPart1.Services
{
    public class LogService : ILogService
    {
        public LogService()
        {
            if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");
        }

        public void WriteVisitAsync(string request)
        {
            WriteSysLogAsync(request, "200");
        }

        public void WriteExceptionAsync(string request, string requestCode)
        {
            WriteSysLogAsync(request, requestCode);
        }

        private static void WriteSysLogAsync( string request, string requestCode)
        {
            try
            {
                var path = $"Logs/app_{DateTime.Now:yyyyMMdd_HHMM}.log";

                if (!File.Exists(path))
                {
                    using var file = File.CreateText(path);
                    file.Close();
                }
                File.AppendAllText(path, $"{requestCode}:{request}\n");

            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
}
    }
}
