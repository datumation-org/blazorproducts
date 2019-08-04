using System;
using log4net;

namespace datumation_products.Shared.Infrastructure.Logging
{
    public class Logger : ILogFactory
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void WriteMessage(string msg)
        {
            log.Debug(msg);
        }

        public void WriteMessage(string msg, Exception ex)
        {
            log.Debug(msg, ex);
        }
        public async void WriteMessageAsync(string msg)
        {
            log.Debug(msg);
        }

    }
}