using System;
using System.IO;
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
		public static void Debug(string message, Type source = null)
		{
			checkConfigInit(source);
            log.Debug(message);
        }

		/// <summary>
		/// Error the specified source and message.
		/// </summary>
		/// <returns>The error.</returns>
		/// <param name="source">Source.</param>
		/// <param name="message">Message.</param>
		public static void Error(string message, Type source = null)
		{
			checkConfigInit(source);
            log.Error(message);
		}

		/// <summary>
		/// Info the specified source and message.
		/// </summary>
		/// <returns>The info.</returns>
		/// <param name="source">Source.</param>
		/// <param name="message">Message.</param>
		public static void Info(string message, Type source = null)
		{
			checkConfigInit(source);
			log.Info(message);
		}
    }
}