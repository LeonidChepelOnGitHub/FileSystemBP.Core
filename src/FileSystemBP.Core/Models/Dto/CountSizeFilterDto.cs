using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSystemBP.Core.Models.Dto
{
    public class CountSizeFilterDto
    {
        public int fromBytes { get; set; }
        public int toBytes { get; set; }
    }
}
