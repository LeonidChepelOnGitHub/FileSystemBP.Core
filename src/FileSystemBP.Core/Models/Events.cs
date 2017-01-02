using System;
using System.Collections.Generic;

namespace FileSystemBP.Core.Models
{
    public partial class Events
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public int? File { get; set; }
        public int? User { get; set; }
        public int? Action { get; set; }
        public DateTime Date { get; set; }

        public virtual Actions ActionNavigation { get; set; }
        public virtual File FileNavigation { get; set; }
        public virtual User UserNavigation { get; set; }
    }
}
