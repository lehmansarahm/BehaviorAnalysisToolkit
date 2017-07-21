using System;
using System.Collections.Generic;
using System.Linq;

namespace BAT.Core.Common
{
    public class CsvFileWriter
	{
        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <param name="input">Input.</param>
		public static void WriteToFile(string filename, ICsvWritable input)
		{
            WriteToFile(filename, input.ToCsv());
		}

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <param name="input">Input.</param>
        public static void WriteToFile(string filename, List<ICsvWritable> input)
		{
            WriteToFile(filename, string.Join("\n", input.Select(x => x.ToCsv())));
		}

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <param name="input">Input.</param>
        private static void WriteToFile(string filename, string input) {
            System.IO.File.Create(filename);
			System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
			file.WriteLine(input);
			file.Close();
        }
	}

	public interface ICsvWritable
	{
        string ToCsv();
	}
}