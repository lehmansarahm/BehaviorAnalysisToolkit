using System;
using System.IO;
using BAT.Core.Common;
using Newtonsoft.Json;

namespace BAT.Core.Config
{
    public class UserInputFile
	{
		[JsonProperty("username")]
		public string Username { get; set; }

		[JsonProperty("source")]
        public string InputSource { get; set; }

        /// <summary>
        /// Gets the input files.
        /// </summary>
        /// <value>The input files.</value>
        public string[] InputFiles
        {
            get
			{
				string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                string currentInput = $"{currentDir}/{InputSource}";

                if (File.Exists(currentInput))
                {
                    // it's a file ... do the thing
                    return new string[] { currentInput };
                }

                if (Directory.Exists(currentInput))
                {
                    string[] files = Directory.GetFiles(@currentInput, 
                                                        $"*{Constants.DEFAULT_INPUT_FILE_EXT}");
                    foreach (var file in files)
                        LogManager.Debug($"Returning input file: {file} for user: {Username}");
                    return files;
                }

                LogManager.Error("Could not locate input files from source: " +
                                 $"{InputSource} for user: {Username}.", this);
                return null;
            }
        }
    }
}