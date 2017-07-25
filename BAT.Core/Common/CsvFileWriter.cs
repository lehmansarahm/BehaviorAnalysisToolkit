using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BAT.Core.Common
{
    public static class CsvFileWriter
    {
		/// <summary>
		/// Writes to file.
		/// </summary>
		/// <param name="outputDirs">Output dirs.</param>
		/// <param name="filename">Filename.</param>
		/// <param name="header">Header.</param>
		/// <param name="input">Input.</param>
		public static void WriteResultsToFile(string[] outputDirs, string filename,
									   string header, IEnumerable<ICsvWritable> input)
		{
			string output = (header + "\n") +
				(input != null ? string.Join("\n", input.Select(x => x.ToCsv())) : "");
			WriteToFile(outputDirs, filename, output);
		}

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="outputDirs">Output dirs.</param>
        /// <param name="filename">Filename.</param>
        /// <param name="header">Header.</param>
        /// <param name="input">Input.</param>
		public static void WriteSummaryToFile(string[] outputDirs, string filename,
                                              string header, IEnumerable<KeyValuePair<string, string>> input,
                                              string footer, string[] footerVals)
		{
            string output = $"{header}\n" +
                (input != null ? string.Join("\n", input.Select(x => x.Key + "," + x.Value)) : "") +
                $"\n{footer}\n{string.Join(",", footerVals)}";
            WriteToFile(outputDirs, filename, output);
		}

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="outputDirs">Output dirs.</param>
        /// <param name="filename">Filename.</param>
        /// <param name="output">Output.</param>
        static void WriteToFile(string[] outputDirs, string filename, string output)
        {
            try
			{
				string outputDir = Constants.OUTPUT_DIR_BY_TIME;
				InitDir(outputDir);

				foreach (var childDir in outputDirs)
				{
					outputDir += $"/{childDir}";
					InitDir(outputDir);
				}

                StreamWriter file = new StreamWriter($"{outputDir}/{filename}");
                file.WriteLine(output);
                file.Close();
            }
            catch (Exception ex)
            {
                LogManager.Error($"Something went wrong while attempting to write output to file: {filename}",
                                 ex, typeof(CsvFileWriter));
            }
        }

        /// <summary>
        /// Inits the dir.
        /// </summary>
        /// <param name="path">Path.</param>
        static void InitDir(string path)
        {
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }
    }
}