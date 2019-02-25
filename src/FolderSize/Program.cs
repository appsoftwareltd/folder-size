using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace FolderSize
{
    class Program
    {
        private static int _recursionLevel;

        private static DirectoryData _directoryData;

        static void Main(string[] args)
        {
            // Using commandline parser to 
            // https://github.com/commandlineparser/commandline

            Parser.Default
                  .ParseArguments<CommandLineOptions>(args)
                  .WithParsed(RunOptionsAndReturnExitCode)
                  .WithNotParsed(HandleParseError);
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            foreach (var error in errs)
            {
                Console.WriteLine(error.ToString());
            }

            Console.ReadLine();
        }

        private static void RunOptionsAndReturnExitCode(CommandLineOptions opts)
        {
            try
            {
                _directoryData = new DirectoryData(opts.SortDirection);

                DirectorySize(opts.SortDirection, new DirectoryInfo(opts.DirectoryPath), _directoryData);

                PrintDirectoryData(opts.RecursionLevel, _directoryData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }

        private static void PrintDirectoryData(int maxRecursionLevel, DirectoryData directoryData)
        {
            if (_recursionLevel < maxRecursionLevel)
            {
                _recursionLevel++;

                string output = null;

                for (int i = 0; i < _recursionLevel; i++)
                {
                    output += "|__";
                }

                output += directoryData.Name + " : " + Math.Round(directoryData.SizeBytes / (double) (1024 * 1024), 2) + " MB";

                Console.WriteLine(output);

                foreach (var subDirectoryData in directoryData.DirectoryDatas)
                {
                    PrintDirectoryData(maxRecursionLevel, subDirectoryData);
                }

                _recursionLevel--;
            }
        }

        private static long DirectorySize(SortDirection sortDirection, DirectoryInfo directoryInfo, DirectoryData directoryData)
        {
            long directorySizeBytes = 0;

            // Add file sizes for current directory

            FileInfo[] fileInfos = directoryInfo.GetFiles();

            foreach (FileInfo fi in fileInfos)
            {
                directorySizeBytes += fi.Length;
            }

            directoryData.Name = directoryInfo.Name;

            directoryData.SizeBytes += directorySizeBytes;

            // Recursively add subdirectory sizes

            DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();

            foreach (DirectoryInfo di in subDirectories)
            {
                var subDirectoryData = new DirectoryData(sortDirection);

                directoryData.DirectoryDatas.Add(subDirectoryData);

                directorySizeBytes += DirectorySize(sortDirection, di, subDirectoryData);
            }

            directoryData.SizeBytes = directorySizeBytes;

            return directorySizeBytes;
        }
    }
}
