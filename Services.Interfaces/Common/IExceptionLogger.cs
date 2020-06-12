using System;

namespace Services.Interfaces.Common
{
    public interface IExceptionLogger
    {
        public void LogException(Exception ex, object additionalParams = null);
    }
}