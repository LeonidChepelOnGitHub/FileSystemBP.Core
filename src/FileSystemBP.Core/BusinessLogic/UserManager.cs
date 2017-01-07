using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FileSystemBP.Core.Models;

namespace FileSystemBP.Core.BusinessLogic
{
    public class UserManager : IUserManager
    {
        public void Add(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public User[] SearchByPredicates<T1, T2>(Expression<Func<bool, T1>> pred1, Expression<Func<bool, T2>> pred2)
        {
            throw new NotImplementedException();
        }

        public void Update(User userToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
