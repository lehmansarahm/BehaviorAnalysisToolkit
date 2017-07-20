using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace BAT.Core.Config
{
    public class Configuration
	{
        public List<string> inputs { get; set; }
		public List<string> transformers { get; set; }
        public List<string> filters { get; set; }
        public List<string> analyzers { get; set; }
        public List<string> summarizers { get; set; }

        /// <summary>
        /// Loads from file.
        /// </summary>
        /// <returns>The from file.</returns>
        /// <param name="filepath">Filepath.</param>
        public static Configuration LoadFromFile(string filepath) {
			using (StreamReader file = File.OpenText(@filepath))
			{
			    JsonSerializer serializer = new JsonSerializer();
                Configuration config = (Configuration)serializer.Deserialize(file, typeof(Configuration));
                return config;
			}
        }
    }
}