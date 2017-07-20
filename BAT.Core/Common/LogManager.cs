using System;

namespace BAT.Core.Common
{
    public class LogManager
    {
        private static readonly log4net.ILog log = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Info the specified message.
        /// </summary>
        /// <returns>The info.</returns>
        /// <param name="message">Message.</param>
        public static void Info(string message) 
        {
            log.Info(message);
        }

        /// <summary>
        /// Debug the specified message.
        /// </summary>
        /// <returns>The debug.</returns>
        /// <param name="message">Message.</param>
        public static void Debug(string message) 
        {
            log.Debug(message);
        }

        /// <summary>
        /// Error the specified message.
        /// </summary>
        /// <returns>The error.</returns>
        /// <param name="message">Message.</param>
        public static void Error(string message) 
        {
            log.Error(message);
        }
    }
}