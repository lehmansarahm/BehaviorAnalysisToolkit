using System;
using System.Collections.Generic;
using System.IO;
using BAT.Core.Common;
using Newtonsoft.Json;

namespace BAT.Core.Config
{
    public class UserInput
	{
		string source;
		List<KeyValuePair<string, string>> files = 
            new List<KeyValuePair<string, string>>();

        [JsonProperty("username")]
        public string Username { get; set; }

		[JsonProperty("source")]
        public string InputSource
        {
            get { return source; }
            set
            {
                source = value;
                SetFiles();
            }
        }

		// key = file name (no ext), value = full file path
		public List<KeyValuePair<string, string>> InputFiles { get { return files; } }

        /// <summary>
        /// Sets the files.
        /// </summary>
        void SetFiles()
		{
			string currentDir = AppDomain.CurrentDomain.BaseDirectory;
			string currentInput = $"{currentDir}/{source}";

			if (File.Exists(currentInput))
			{
                // it's a file ... do the thing
                string filename = GetFilenameFromPath(currentInput);
                files.Add(new KeyValuePair<string, string>(filename, currentInput));
			}
            else if (Directory.Exists(currentInput))
			{
				var paths = Directory.GetFiles(@currentInput,
                                           $"*{Constants.DEFAULT_INPUT_FILE_EXT}");
                foreach (var path in paths)
				{
					LogManager.Debug($"Returning input file: {path} for user: {Username}");
					string filename = GetFilenameFromPath(path);
					files.Add(new KeyValuePair<string, string>(filename, path));
                }
			}
            else 
			    LogManager.Error("Could not locate input files from source: " +
							 $"{InputSource} for user: {Username}.", this);
        }

        /// <summary>
        /// Gets the filename from path.
        /// </summary>
        /// <returns>The filename from path.</returns>
        /// <param name="path">Path.</param>
        string GetFilenameFromPath(string path)
		{
            var pathComponents = path.Split(Constants.DEFAULT_PATH_SEPARATOR);
            var filename = Username + Constants.DEFAULT_NAME_SEPARATOR
                           + pathComponents[pathComponents.Length - 1];
            return filename.Substring(0, filename.IndexOf(Constants.DEFAULT_INPUT_FILE_EXT));
        }
    }
}