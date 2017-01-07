
using FileSystemBP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileSystemBP.Core.BusinessLogic
{
    public interface IEventService
    {
        Events GetById(int id);
        Events[] Search<T1, T2>(Expression<Func<bool, T1>> pred1, Expression<Func<bool, T2>> pred2);
        
    }
}
