using System;
using BAT.Core.Common;
using BAT.Core.Config;

namespace BAT.MCI
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string configFilepath = "configuration.json";
            if (args.Length >= 1) configFilepath = args[0]; 
            else LogManager.Info("No configuration file provided.  Using default.");

            try
			{
				var config = Configuration.LoadFromFile(configFilepath);
				LogManager.Info("Configuration file loaded.");
            } catch (Exception e) {
                LogManager.Error("ERROR:  Could not load configuration file.  Exiting program.");
            }
        }
    }
}