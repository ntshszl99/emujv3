using System;
using NLog.Web;

namespace Shared
{
    public class LogError
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void WriteError(string ErrorLog)
        {
            //var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                string salah = DateTime.Today.ToString() + System.Environment.NewLine + ErrorLog;
                logger.Error(salah);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
            logger.Debug("init main");
        }

    }
}
