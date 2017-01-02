using System;
using System.Collections.Generic;

namespace FileSystemBP.Core.Models
{
    public partial class History
    {
        public History()
        {
            BusinessProcessHistory = new HashSet<BusinessProcessHistory>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int? Modified { get; set; }

        public virtual ICollection<BusinessProcessHistory> BusinessProcessHistory { get; set; }
        public virtual File ModifiedNavigation { get; set; }
    }
}
