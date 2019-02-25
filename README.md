# App Software Folder Size

App Software Folder size is a command line utility written in .NET Core for reporting on folder size from a given root directory, reporting to a given depth.

To hire me, email mail@appsoftware.com

## Options:

### Set the root path from which to report folder size

-d or --directory-path 

Example: C:\test-directory
Required: Yes

### Set the number of levels down the directory hierarchy to report on

Note: The directory structure is fully recursed for file and folder size reporting regardless of the value passed here.

-r or --recursion-level 

Example: 4
Required: No
Default: 2

## Set the sort direction (by size)

Values that can be passed are `asc` (Ascending), `desc` (Descending) or `none` (No sort)

-s or --sort-direction

Example: asc
Required: No
Default: none

To build for Windows platforms, run the file Build-win-x64 or run the following in the project directory. 

	dotnet publish -r win-x64 -c release