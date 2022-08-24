// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryList.cs" company="Software Inc.">
//   A.Robson
// </copyright>
// <summary>
//   Create a command file with a directory rename option.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Dlist
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    /// The directory list.
    /// </summary>
    public class DirectoryList
    {
        public static int Main(string[] args)
        {
            var di = Directory.GetCurrentDirectory();

            var arrayList = new ArrayList();
            arrayList = GetDirectoryList(di);

            var len = GetLongestLine(arrayList);

            WriteCommand(arrayList, len);

            return 0;
        }

        /// <summary>
        /// Write the commmand file.
        /// </summary>
        /// <param name="arrayList">The array list.</param>
        /// <param name="lineLength">The line length.</param>
        private static void WriteCommand(ArrayList arrayList, int lineLength)
        {
            var outStream = File.Create(@"alan.cmd");
            var sw = new StreamWriter(outStream);
            
            foreach (string file in arrayList)
            {
                var space = string.Empty;
                space += space.PadRight(lineLength - Path.GetFileName(file).Length);
                var newName = CleanFilename(Path.GetFileName(file));
                var str = "ren \"" + Path.GetFileName(file) + "\"" + space + " \"" + newName + "\"";
                
                sw.WriteLine(str);
            }

            // flush and close
            sw.Flush();
            sw.Close();

            Console.WriteLine("Finished...");
        }

        /// <summary>
        /// Clean up the filename.
        /// </summary>
        /// <param name="file">The file name.</param>
        /// <returns>The <see cref="string"/>cleaned filename.</returns>
        private static string CleanFilename(string file)
        {
            var singleDigit = @"^[0-9]\. ";
            var singleDigit2 = @"^[0-9]\.";
            var doubleDigit = @"^[1-9][0-9]\. ";
            var doubleDigit2 = @"^[1-9][0-9]\.";
            var zeroBasedDigit = @"^[0-9][0-9]\. ";
            var zeroBasedDigit2 = @"^[0-9][0-9]\.";
            var dashBasedDigit = @"^[1-9] - ";
            var dashBasedDigit2 = @"^[1-9]- ";
            var doubleDashDigit = @"^[0-9][0-9] - ";
            var doubleDashDigit2 = @"^[0-9][0-9]- ";
            
            if (file.Contains("!"))
            {
                file = file.Replace("!", string.Empty);
            }

            if (Regex.IsMatch(file, singleDigit))
            {
                file = file.Replace(". ", "-");
                file = "0" + file;
            }

            if (Regex.IsMatch(file, singleDigit2))
            {
                file = file.Replace(".", "-");
                file = "0" + file;
            }

            if (Regex.IsMatch(file, doubleDigit))
            {
                file = file.Replace(". ", "-");
            }

            if (Regex.IsMatch(file, doubleDigit2))
            {
                file = file.Replace(".", "-");
            }

            if (Regex.IsMatch(file, zeroBasedDigit))
            {
                file = file.Replace(". ", "-");
            }

            if (Regex.IsMatch(file, zeroBasedDigit2))
            {
                file = file.Replace(".", "-");
            }

            if (Regex.IsMatch(file, dashBasedDigit))
            {
                file = file.Replace(" - ", "-");
                file = "0" + file;
            }

            if (Regex.IsMatch(file, dashBasedDigit2))
            {
                file = file.Replace("- ", "-");
                file = "0" + file;
            }

            if (Regex.IsMatch(file, doubleDashDigit))
            {
                file = file.Replace(" - ", "-");
            }

            if (Regex.IsMatch(file, doubleDashDigit2))
            {
                file = file.Replace("- ", "-");
            }

            return file;
        }

        /// <summary>
        /// Get the directory list.
        /// </summary>
        /// <param name="di">The directory.</param>
        /// <returns>The <see cref="ArrayList"/>array list.</returns>
        private static ArrayList GetDirectoryList(string di)
        {
            var directoryPattern = "*";
            var arrayList = new ArrayList();

            try
            {
                var directories = Directory.GetDirectories(di, directoryPattern, SearchOption.TopDirectoryOnly);

                foreach (var dir in directories)
                {
                    if (Path.GetFileName(dir) != null)
                    {
                        arrayList.Add(Path.GetFileName(dir));    
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

            return arrayList;
        }

        /// <summary>
        /// The get longest directory name.
        /// </summary>
        /// <param name="arrayList">The array list.</param>
        /// <returns>The <see cref="int"/>longest line.</returns>
        private static int GetLongestLine(ArrayList arrayList)
        {
            int lineLength = 0;

            foreach (string line in arrayList)
            {
                if (Path.GetFileName(line) != null)
                {
                    var directoryName = Path.GetFileName(line);

                    if (directoryName.Length > lineLength)
                    {
                        lineLength = directoryName.Length;
                    }

                }
            }

            return lineLength;
        }
    }
}
