using System;
using System.Collections.Generic;

namespace FileSystemBP.Core.Models
{
    public partial class ProcessType
    {
        public ProcessType()
        {
            BusinessProcess = new HashSet<BusinessProcess>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<BusinessProcess> BusinessProcess { get; set; }
    }
}
