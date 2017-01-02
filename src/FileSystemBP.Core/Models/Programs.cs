using System;
using System.Collections.Generic;

namespace FileSystemBP.Core.Models
{
    public partial class Programs
    {
        public Programs()
        {
            BusinessProcessPrograms = new HashSet<BusinessProcessPrograms>();
            File = new HashSet<File>();
            ProgramFileTypeFile = new HashSet<ProgramFileTypeFile>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<BusinessProcessPrograms> BusinessProcessPrograms { get; set; }
        public virtual ICollection<File> File { get; set; }
        public virtual ICollection<ProgramFileTypeFile> ProgramFileTypeFile { get; set; }
    }
}
