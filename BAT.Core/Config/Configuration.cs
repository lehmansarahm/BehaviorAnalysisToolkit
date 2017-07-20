using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace BAT.Core.Config
{
    public class Configuration
	{
        [JsonProperty("inputs")]
		public List<string> Inputs { get; set; }

		[JsonProperty("transformers")]
		public List<string> Transformers { get; set; }

		[JsonProperty("filters")]
		public List<string> Filters { get; set; }

		[JsonProperty("analyzers")]
		public List<string> Analyzers { get; set; }

		[JsonProperty("summarizers")]
        public List<string> Summarizers { get; set; } 

		/// <summary>
		/// Loads from file.
		/// </summary>
		/// <returns>The from file.</returns>
		/// <param name="filepath">Filepath.</param>
		public static Configuration LoadFromFile(string filepath)
		{
			var config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(filepath));
            return config;
        }
    }
}