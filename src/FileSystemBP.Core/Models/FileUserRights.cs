using System;
using System.Collections.Generic;

namespace FileSystemBP.Core.Models
{
    public partial class FileUserRights
    {
        public int File { get; set; }
        public int User { get; set; }
        public int Rights { get; set; }

        public virtual File FileNavigation { get; set; }
        public virtual Access RightsNavigation { get; set; }
        public virtual User UserNavigation { get; set; }
    }
}
