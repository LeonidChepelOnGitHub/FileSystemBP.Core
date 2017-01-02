using System;
using System.Collections.Generic;

namespace FileSystemBP.Core.Models
{
    public partial class SecurityLevel
    {
        public SecurityLevel()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Level { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
