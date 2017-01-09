using FileSystemBP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileSystemBP.Core.BusinessLogic
{
    public interface IUserManager
    {
        User GetById(int id);
        void Add(User user);
        void Delete(int id);
        void Update(User userToUpdate);
        User[] SearchByPredicates(Expression<Func<User, bool>> pred1, Expression<Func<User, bool>> pred2);

    }
}
