using System;
using System.Collections.Generic;

namespace FileSystemBP.Core.Models
{
    public partial class FileType
    {
        public FileType()
        {
            ProgramFileTypeFile = new HashSet<ProgramFileTypeFile>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<ProgramFileTypeFile> ProgramFileTypeFile { get; set; }
    }
}
