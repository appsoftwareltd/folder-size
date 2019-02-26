using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace FolderSize
{
    class Program
    {
        private static int _recursionLevel;

        private static int _directoriesProcessed = 0;

        private static DirectoryData _directoryData;

        static void Main(string[] args)
        {
            Console.WriteLine("Working. Directories processed: ");

            // Using commandline parser to 
            // https://github.com/commandlineparser/commandline

            Parser.Default
                  .ParseArguments<CommandLineOptions>(args)
                  .WithParsed(RunOptionsAndReturnExitCode)
                  .WithNotParsed(HandleParseError);

#if DEBUG
            Console.ReadLine();
#endif
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            foreach (var error in errs)
            {
                Console.WriteLine(error.ToString());
            }
        }

        private static void RunOptionsAndReturnExitCode(CommandLineOptions opts)
        {
            try
            {
                _directoryData = new DirectoryData(opts.SortDirection);

                DirectorySize(opts.SortDirection, new DirectoryInfo(opts.DirectoryPath), _directoryData);

                Console.WriteLine(Environment.NewLine);

                PrintDirectoryData(opts.RecursionLevel, _directoryData);

                PrintProgress();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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

                double directorySizeMb = Math.Round(directoryData.SizeBytes / (double) (1024 * 1024), 2);

                output += $"{directoryData.Name} : { directorySizeMb } MB";

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

            directoryData.Name = directoryInfo.Name;

            try
            {
                // Add file sizes for current directory

                FileInfo[] fileInfos = directoryInfo.GetFiles();

                foreach (FileInfo fileInfo in fileInfos)
                {
                    directorySizeBytes += fileInfo.Length;
                }

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
            }
            catch (UnauthorizedAccessException uaex)
            {
                directoryData.Name += " (Unable to calculate size - Unauthorised)";
            }
            catch (Exception ex)
            {
                directoryData.Name += " (Unable to calculate size - Error)";
            }

            _directoriesProcessed++;

            if (_directoriesProcessed % 10 == 0)
            {
                PrintProgress();
            }

            return directorySizeBytes;
        }

        private static void PrintProgress()
        {
            Console.SetCursorPosition(32, 0);

            Console.Write(_directoriesProcessed);
        }
    }
}
