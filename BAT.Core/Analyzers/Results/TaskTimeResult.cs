using System;
using BAT.Core.Common;

namespace BAT.Core.Analyzers.Results
{
	public class TaskTimeResult : ICsvWritable
	{
		public static string[] Header
		{
			get
			{
				return new string[]
				{
					"Start Time",
					"Start Num",
					"End Time",
					"End Num",
					"Task Duration"
				};
			}
		}
		public static string HeaderCsv { get { return string.Join(",", Header); } }

		//----------------------------------------------------------------------
		//----------------------------------------------------------------------

		public DateTime Start { get; set; }
		public int StartNum { get; set; }
		public DateTime End { get; set; }
		public int EndNum { get; set; }
		public double Duration { get; set; }

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