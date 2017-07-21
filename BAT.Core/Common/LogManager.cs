using System;
using log4net.Config;

namespace BAT.Core.Common
{
    public class LogManager
	{
        private static log4net.ILog log;
        private static bool configInitialized = false;

        /// <summary>
        /// Checks the config init.
        /// </summary>
        /// <param name="source">Source.</param>
        private static void checkConfigInit(Type source)
		{
			log = log4net.LogManager.GetLogger(source ?? System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            if (!configInitialized)
            {
				BasicConfigurator.Configure();
                if (log4net.LogManager.GetRepository().Configured) configInitialized = true; 
                else Console.WriteLine("Could not initialize Log4Net config.");
            }
        }

		/// <summary>
		/// Debug the specified source and message.
		/// </summary>
		/// <returns>The debug.</returns>
		/// <param name="source">Source.</param>
		/// <param name="message">Message.</param>
		public static void Debug(string message, object source = null)
		{
			checkConfigInit(source?.GetType());
            log.Debug(message);
		}

		/// <summary>
		/// Error the specified source and message.
		/// </summary>
		/// <returns>The error.</returns>
		/// <param name="source">Source.</param>
		/// <param name="message">Message.</param>
		public static void Error(string message, object source = null)
		{
			checkConfigInit(source?.GetType());
			log.Error(message);
		}

		/// <summary>
		/// Error the specified source and message.
		/// </summary>
		/// <returns>The error.</returns>
		/// <param name="source">Source.</param>
		/// <param name="message">Message.</param>
		public static void Error(string message, Exception e, object source = null)
		{
			checkConfigInit(source?.GetType());
			log.Error(message);
			log.Error("EXCEPTION OUTPUT:\n" + e.Message 
                      + "\n-------------------------------------------\n"
                      + e.StackTrace + ".....\n\n");
		}

		/// <summary>
		/// Info the specified source and message.
		/// </summary>
		/// <returns>The info.</returns>
		/// <param name="source">Source.</param>
		/// <param name="message">Message.</param>
		public static void Info(string message, object source = null)
		{
			checkConfigInit(source?.GetType());
			log.Info(message);
		}
    }
}