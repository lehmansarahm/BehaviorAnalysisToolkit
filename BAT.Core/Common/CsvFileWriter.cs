using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BAT.Core.Constants;

namespace BAT.Core.Common
{
    public static class CsvFileWriter
    {
        /// <summary>
        /// Copies the file to dir.
        /// </summary>
        /// <param name="filename">Filepath.</param>
        /// <param name="outputDir">Output dir.</param>
        public static void CopyFileToDir(string filename, string outputDir)
		{
			string currentDir = AppDomain.CurrentDomain.BaseDirectory + Constants.BAT.DEFAULT_PATH_SEPARATOR;
            string currentFilepath = currentDir + filename;

            if (File.Exists(currentFilepath))
            {
                InitDir(outputDir);
                string targetFilepath = currentDir + outputDir 
                    + Constants.BAT.DEFAULT_PATH_SEPARATOR + filename;
                File.Copy(currentFilepath, targetFilepath);
            }
        }

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
                                              string header, IEnumerable<string[]> input,
                                              string footer, string[] footerVals)
		{
            string output = $"{header}\n" +
                (input != null ? string.Join("\n", input.Select(x => string.Join(",", x))) : "") +
                $"\n{string.Join(",", GetEmptyLine(footerVals.Length))}" +
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
                string outputDir = OutputDirs.ExecTime;
				InitDir(outputDir);

				foreach (var childDir in outputDirs)
				{
					outputDir += $"/{childDir}";
					InitDir(outputDir);
				}

                if (!filename.EndsWith(Constants.BAT.DEFAULT_INPUT_FILE_EXT))
                    filename = (filename + Constants.BAT.DEFAULT_INPUT_FILE_EXT);

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

        /// <summary>
        /// Gets the empty line.
        /// </summary>
        /// <returns>The empty line.</returns>
        /// <param name="count">Count.</param>
        static List<string> GetEmptyLine(int count)
        {
            var output = new List<string>();
            for (int i = 0; i < count; i++) output.Add("");
            return output;
        }
    }
}