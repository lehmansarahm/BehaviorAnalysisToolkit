using System;
using BAT.Core.Common;

namespace BAT.Core.Analyzers.Results
{
	public class TaskTimeResult : ICsvWritable
	{
		public DateTime Start { get; set; }
		public int StartNum { get; set; }
		public DateTime End { get; set; }
		public int EndNum { get; set; }
		public decimal Duration { get; set; }

		/// <summary>
		/// Tos the csv.
		/// </summary>
		/// <returns>The csv.</returns>
		public string ToCsv()
		{
			string[] props = {
				Start.ToString(),
				StartNum.ToString(),
				End.ToString(),
				EndNum.ToString(),
				Duration.ToString()
			};
			return string.Join(",", props);
		}
	}
}