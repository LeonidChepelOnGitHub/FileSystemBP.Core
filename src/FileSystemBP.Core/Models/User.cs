using System;
using System.Collections.Generic;

namespace FileSystemBP.Core.Models
{
    public partial class User
    {
        public User()
        {
            BusinessProcess = new HashSet<BusinessProcess>();
            Events = new HashSet<Events>();
            File = new HashSet<File>();
            FileUserRights = new HashSet<FileUserRights>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int? SecurityLevel { get; set; }

        public virtual ICollection<BusinessProcess> BusinessProcess { get; set; }
        public virtual ICollection<Events> Events { get; set; }
        public virtual ICollection<File> File { get; set; }
        public virtual ICollection<FileUserRights> FileUserRights { get; set; }
        public virtual SecurityLevel SecurityLevelNavigation { get; set; }
    }
}
