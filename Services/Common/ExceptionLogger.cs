using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Services.Interfaces.Common;

namespace Services.Common
{
    public class ExceptionLogger: IExceptionLogger
    {
        private object bolt = new object();

        public void LogException(Exception ex, object additionalParams = null)
        {
            var exceptionContent = $"{DateTime.UtcNow}. Encountered an exception: {CollectExceptionMessages("", ex)}.\nStack trace: {ex.StackTrace}";
            
            WriteToFile(exceptionContent);
        }

        private string CollectExceptionMessages(string content, Exception ex)
        {
            if (ex.InnerException == null)
            {
                return content + '\n' + ex.Message;
            }
            
            return content + '\n' + CollectExceptionMessages(content, ex.InnerException);
        }

        private void WriteToFile(string content)
        {
            lock (bolt)
            {
                var fullPath = Path.Join(Directory.GetCurrentDirectory(), DateTime.UtcNow.ToString("yyyy_MM_dd")
                    .Replace(' ', '_')
                    .Replace('/', '_') + "_api.log");
                
                using (var file = new StreamWriter(fullPath, true, Encoding.UTF8))
                {
                    file.WriteLineAsync(content);
                }
            }
        }
    }
}