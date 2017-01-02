using System;
using System.Collections.Generic;

namespace FileSystemBP.Core.Models
{
    public partial class BusinessProcessPrograms
    {
        public int Program { get; set; }
        public int BusinessProcess { get; set; }

        public virtual BusinessProcess BusinessProcessNavigation { get; set; }
        public virtual Programs ProgramNavigation { get; set; }
    }
}
