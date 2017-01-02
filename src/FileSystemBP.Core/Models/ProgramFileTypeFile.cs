using System;
using System.Collections.Generic;

namespace FileSystemBP.Core.Models
{
    public partial class ProgramFileTypeFile
    {
        public int File { get; set; }
        public int FileType { get; set; }
        public int Program { get; set; }

        public virtual File FileNavigation { get; set; }
        public virtual FileType FileTypeNavigation { get; set; }
        public virtual Programs ProgramNavigation { get; set; }
    }
}
