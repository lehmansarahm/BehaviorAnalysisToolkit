using System;
using System.IO;
using System.Linq;

namespace BAT.Core.Test
{
    public partial class BATTest
    {
        private const string TEST_DATA_FOLDER = "SupportFiles";
        private const string TEST_DATA_FOLDER_CONFIGS = "ConfigFiles";
        private const string TEST_DATA_FOLDER_INPUTS = "InputFiles";

        /// <summary>
        /// Gets the config file path.
        /// </summary>
        /// <returns>The config file path.</returns>
        /// <param name="filename">Filename.</param>
        protected static string GetConfigFilePath(string filename)
        {
            return Path.Combine(GetTestDataFolder(TEST_DATA_FOLDER_CONFIGS), filename);
        }

		/// <summary>
		/// Gets the input file path.
		/// </summary>
		/// <returns>The input file path.</returns>
		/// <param name="filename">Filename.</param>
		protected static string GetInputFilePath(string filename)
		{
			return Path.Combine(GetTestDataFolder(TEST_DATA_FOLDER_INPUTS), filename);
        }

        /// <summary>
        /// Gets the test data folder.
        /// </summary>
        /// <returns>The test data folder.</returns>
        /// <param name="testDataFolder">Test data folder.</param>
		private static string GetTestDataFolder(string testDataFolder)
		{
			string startupPath = AppDomain.CurrentDomain.BaseDirectory;
			var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
			var pos = pathItems.Reverse().ToList().FindIndex(x => string.Equals("bin", x));
			string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - pos - 1));
			return Path.Combine(projectPath, TEST_DATA_FOLDER, testDataFolder);
		}
    }
}