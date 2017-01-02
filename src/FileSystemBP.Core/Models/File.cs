using System;
using System.Collections.Generic;

namespace FileSystemBP.Core.Models
{
    public partial class File
    {
        public File()
        {
            Events = new HashSet<Events>();
            FileSystemFile = new HashSet<FileSystem>();
            FileSystemParent = new HashSet<FileSystem>();
            FileUserRights = new HashSet<FileUserRights>();
            History = new HashSet<History>();
            ProgramFileTypeFile = new HashSet<ProgramFileTypeFile>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? ProgramDefault { get; set; }
        public int Size { get; set; }
        public int? Owner { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<Events> Events { get; set; }
        public virtual ICollection<FileSystem> FileSystemFile { get; set; }
        public virtual ICollection<FileSystem> FileSystemParent { get; set; }
        public virtual ICollection<FileUserRights> FileUserRights { get; set; }
        public virtual ICollection<History> History { get; set; }
        public virtual ICollection<ProgramFileTypeFile> ProgramFileTypeFile { get; set; }
        public virtual User OwnerNavigation { get; set; }
        public virtual Programs ProgramDefaultNavigation { get; set; }
    }
}
