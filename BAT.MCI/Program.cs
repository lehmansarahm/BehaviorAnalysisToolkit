using System;
using BAT.Core.Common;
using BAT.Core.Config;

namespace BAT.MCI
{
    class Program
    {
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            Configuration config;
            try {
				string configFilepath = Constants.DEFAULT_CONFIG_FILE;
				if (args.Length >= 1) configFilepath = args[0];
				else LogManager.Info("No configuration file provided.  Using default.");

				config = Configuration.LoadFromFile(configFilepath);
                config.WriteOutputFile = true;
				LogManager.Info($"Configuration file loaded from {configFilepath}");
            } catch (Exception e) {
                LogManager.Error("ERROR:  Could not load configuration file.  "
                                 + "Exiting program.", e, typeof(Program));
                return;
			}

			bool success = config.LoadInputs();
			if (success) LogManager.Info("Input data successfully loaded.");
			else {
				LogManager.Error("Something went wrong while loading the input data.  Exiting program.");
				return;
			}

			success = config.RunTransformers();
			if (success) LogManager.Info("Selected transformations successfully run on input data.");
			else {
				LogManager.Error("Something went wrong while running transformations.  Exiting program.");
				return;
			}

			success = config.RunFilters();
			if (success) LogManager.Info("Selected filters successfully run on input data.");
			else {
				LogManager.Error("Something went wrong while running filters.  Exiting program.");
				return;
			}

			success = config.RunAnalyzers();
			if (success) LogManager.Info("Selected analysis operations successfully run on input data.");
			else {
				LogManager.Error("Something went wrong while running analysis operations.  Exiting program.");
				return;
			}
        }
    }
}