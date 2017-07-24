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
        /// <param name="phaseOutputDir">Phase output dir.</param>
        /// <param name="operationOutputDir">Operation output dir.</param>
        /// <param name="filename">Filename.</param>
        /// <param name="header">Header.</param>
        /// <param name="input">Input.</param>
		public static void WriteToFile(string phaseOutputDir, string operationOutputDir, 
                                       string filename, string header, IEnumerable<ICsvWritable> input)
		{
			string phaseDir = $"{Constants.OUTPUT_DIR_BY_TIME}/{phaseOutputDir}";
			string operationDir = $"{phaseDir}/{operationOutputDir}";

			if (!Directory.Exists(Constants.OUTPUT_DIR_BY_TIME)) 
                Directory.CreateDirectory(Constants.OUTPUT_DIR_BY_TIME);
			if (!Directory.Exists(phaseDir)) Directory.CreateDirectory(phaseDir);
            if (!Directory.Exists(operationDir)) Directory.CreateDirectory(operationDir);

            string outputFilePath = $"{operationDir}/{filename}";
            string output = (header + "\n") +
                (input != null ? string.Join("\n", input.Select(x => x.ToCsv())) : "");
            WriteToFile(outputFilePath, output);
        }

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="filepath">Filename.</param>
        /// <param name="output">Input.</param>
        static void WriteToFile(string filepath, string output)
        {
            try
            {
                StreamWriter file = new StreamWriter(filepath);
                file.WriteLine(output);
                file.Close();
            }
            catch (Exception ex)
            {
                LogManager.Error($"Something went wrong while attempting to write output to file: {filepath}",
                                 ex, typeof(CsvFileWriter));
            }
        }
    }
}