using System;
using System.Collections.Generic;

namespace FileSystemBP.Core.Models
{
    public partial class Access
    {
        public Access()
        {
            FileUserRights = new HashSet<FileUserRights>();
        }

        public int Id { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Create { get; set; }
        public bool Delete { get; set; }

        public virtual ICollection<FileUserRights> FileUserRights { get; set; }
    }
}
