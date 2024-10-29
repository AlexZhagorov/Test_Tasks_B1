using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    internal class FileMerger
    {
        public static void MergeFiles()
        {
            Console.WriteLine("Enter the path to files to merge:");
            string path_to_files = Console.ReadLine();

            Console.WriteLine("Enter the path to save the merged file:");
            string path_to_save = Console.ReadLine();

            string[] files = Directory.GetFiles(path_to_files, "file_*.txt");
            string pattern;

            Console.WriteLine("Write the pattern to delete or press Enter to skip:");
            pattern = Console.ReadLine();

            using (StreamWriter writer = new StreamWriter(path_to_save, false, Encoding.UTF8))
            {
                if (pattern != string.Empty)
                {
                    int omitted_lines = 0;
                    foreach (string file in files)
                    {
                        foreach (string line in File.ReadLines(file))
                        {
                            if (!line.Contains(pattern))
                            {
                                writer.WriteLine(line);
                            }
                            else
                            {
                                omitted_lines++;
                            }
                        }
                    }
                    Console.WriteLine($"{omitted_lines} lines omitted.");
                    Console.WriteLine("Files merged successfully.");
                }
                else
                {
                    foreach (string file in files)
                    {
                        foreach (string line in File.ReadLines(file))
                        {
                            writer.WriteLine(line);
                        }
                    }
                    Console.WriteLine("Files merged successfully.");
                }
            }
        }
    }
}
