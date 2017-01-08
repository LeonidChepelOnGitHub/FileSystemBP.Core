using FileSystemBP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileSystemBP.Core.BusinessLogic
{
    public interface IProgramManager
    {
        Programs GetById(int id);
        void Add(Programs program, FileType filetype);
        void Add(Programs program, FileType filetype,int[] fileIds);

        void Delete(int id);
        void Update(Programs programToUpdate);
        Programs[] SearchByPredicate(Expression<Func<Programs, bool>> pred);
        Programs[] SearchByFileType(FileType filetype);
        
    }
}
