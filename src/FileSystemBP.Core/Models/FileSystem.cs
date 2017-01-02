using System;
using System.Collections.Generic;

namespace FileSystemBP.Core.Models
{
    public partial class FileSystem
    {
        public int FileId { get; set; }
        public int ParentId { get; set; }

        public virtual File File { get; set; }
        public virtual File Parent { get; set; }
    }
}
