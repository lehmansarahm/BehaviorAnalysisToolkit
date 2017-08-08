using System;

namespace BAT.Core.Constants
{
    public static class OutputDirs
	{
		public static string ExecDateTime = DateTime.Now.ToString("MMddyyyy-hhmmss");
		public static string ExecTime = $"output-{ExecDateTime}";

		public const string Analyzers = "analysis";
		public const string Filters = "filters";
		public const string Inputs = "input";
		public const string Summarizers = "summaries";
		public const string Transformers = "transforms";
    }
}