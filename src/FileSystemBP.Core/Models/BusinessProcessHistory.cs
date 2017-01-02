using System;
using System.Collections.Generic;

namespace FileSystemBP.Core.Models
{
    public partial class BusinessProcessHistory
    {
        public int History { get; set; }
        public int BusinessProcess { get; set; }

        public virtual BusinessProcess BusinessProcessNavigation { get; set; }
        public virtual History HistoryNavigation { get; set; }
    }
}
