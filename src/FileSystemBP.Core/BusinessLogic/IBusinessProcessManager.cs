using FileSystemBP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileSystemBP.Core.BusinessLogic
{
    public interface IBusinessProcessManager
    {
        BusinessProcess GetById(int id);
        void Add(BusinessProcess toAdd, Program prog);
        void Delete(int id);
        void Update(BusinessProcess toUpdate, Program prog);
        BusinessProcess[] Search<T1, T2>(Expression<Func<bool, T1>> pred1,
            Expression<Func<bool, T2>> pred2,Program prog, int? uId);
    }
}
