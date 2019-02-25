using System.Collections.Generic;
using System.Linq;

namespace FolderSize
{
    public class DirectoryData
    {
        private readonly SortDirection _sortDirection; 

        private IList<DirectoryData> _directoryDatas;

        public DirectoryData(SortDirection sortDirection)
        {
            _sortDirection = sortDirection;

            _directoryDatas = new List<DirectoryData>();
        }

        public string Name { get; set; }

        public long SizeBytes { get; set; }

        public IList<DirectoryData> DirectoryDatas
        {
            get
            {
                if (_sortDirection == SortDirection.asc)
                {
                    _directoryDatas = _directoryDatas.OrderBy(x => x.SizeBytes).ToList();
                }
                else if (_sortDirection == SortDirection.desc)
                {
                    _directoryDatas = _directoryDatas.OrderByDescending(x => x.SizeBytes).ToList();
                }
                
                // Else no sort

                return _directoryDatas;
            }
        }
    }
}
