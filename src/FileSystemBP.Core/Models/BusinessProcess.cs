using System;
using System.Collections.Generic;

namespace FileSystemBP.Core.Models
{
    public partial class BusinessProcess
    {
        public BusinessProcess()
        {
            BusinessProcessHistory = new HashSet<BusinessProcessHistory>();
            BusinessProcessPrograms = new HashSet<BusinessProcessPrograms>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LastRun { get; set; }
        public string Description { get; set; }
        public int? Type { get; set; }
        public int? User { get; set; }

        public virtual ICollection<BusinessProcessHistory> BusinessProcessHistory { get; set; }
        public virtual ICollection<BusinessProcessPrograms> BusinessProcessPrograms { get; set; }
        public virtual ProcessType TypeNavigation { get; set; }
        public virtual User UserNavigation { get; set; }
    }
}
