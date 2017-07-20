namespace BAT.Core.Test
{
    public partial class BATTest
    {
        private const string CONFIG_FILEPATH = "SupportFiles/ConfigFiles/";
        private const string INPUT_FILEPATH = "SupportFiles/InputFiles/";

        /// <summary>
        /// Gets the config file path.
        /// </summary>
        /// <returns>The config file path.</returns>
        /// <param name="filename">Filename.</param>
        protected static string GetConfigFilePath(string filename) {
            return CONFIG_FILEPATH + filename;
        }

        /// <summary>
        /// Gets the input file path.
        /// </summary>
        /// <returns>The input file path.</returns>
        /// <param name="filename">Filename.</param>
        protected static string GetInputFilePath(string filename) {
            return INPUT_FILEPATH + filename;
        }
    }
}