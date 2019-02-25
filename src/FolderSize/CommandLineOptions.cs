using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace FolderSize
{
    public class CommandLineOptions
    {
        [Option('d', "directory-path", Required = true, HelpText = "Directory path")]
        public string DirectoryPath { get; set; }

        [Option('r', "recursion-level", Required = false, Default = 2, HelpText = "Folder depth")]
        public int RecursionLevel { get; set; }

        [Option('s', "sort-direction", Required = false, Default = SortDirection.desc, HelpText = "Sort direction (by folder size)")]
        public SortDirection SortDirection { get; set; }
    }
}
