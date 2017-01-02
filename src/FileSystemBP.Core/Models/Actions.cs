using System;
using System.Collections.Generic;

namespace FileSystemBP.Core.Models
{
    public partial class Actions
    {
        public Actions()
        {
            Events = new HashSet<Events>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Events> Events { get; set; }
    }
}
