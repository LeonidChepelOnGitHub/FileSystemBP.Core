using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FileSystemBP.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSystemBP.Core.BusinessLogic
{
    public class UserManager : IUserManager
    {
        public void Add(User user)
        {
            using (var context = new FileSystemContext())
            {
                SecurityLevel dbnav = null;
                if (user.SecurityLevelNavigation != null)
                {
                    var snav = user.SecurityLevelNavigation;
                    user.SecurityLevelNavigation = null;
                    dbnav = context.SecurityLevel.FirstOrDefault(l => l.Level == snav.Level);
                    if (dbnav == null)
                    {
                        context.SecurityLevel.Add(snav);
                        context.SaveChanges();
                        dbnav = snav;
                    }
                    user.SecurityLevel = dbnav.Id;
                }
                context.User.Add(user);
                context.SaveChanges();
                user.SecurityLevelNavigation = dbnav;
            }
        }

        public void Delete(int id)
        {
            using (var context = new FileSystemContext())
            {
                var user = context.User.FirstOrDefault(u => u.Id == id);
                context.User.Remove(user);
                context.SaveChanges();
            }
        }

        public User GetById(int id)
        {
            using (var context = new FileSystemContext())
            {
                var user = context.User
                    .Include(u=>u.SecurityLevelNavigation)                
                    .FirstOrDefault(u => u.Id == id);
                return user;
            }
        }

        public User[] SearchByPredicates(Expression<Func<User, bool>> pred1, Expression<Func<User, bool>> pred2)
        {
            using (var context = new FileSystemContext())
            {
                var search = context.User
                    .Include(u => u.SecurityLevelNavigation)
                    .AsQueryable();
                if(pred1!=null)
                {
                    search = search.Where(pred1);
                }
                if (pred2 != null)
                {
                    search = search.Where(pred2);
                }

                return search.ToArray();
            }
        }

        public void Update(User userToUpdate)
        {
            using (var context = new FileSystemContext())
            {
                SecurityLevel dbnav = null;
                if (userToUpdate.SecurityLevelNavigation != null)
                {
                    var snav = userToUpdate.SecurityLevelNavigation;
                    userToUpdate.SecurityLevelNavigation = null;
                    dbnav = context.SecurityLevel.FirstOrDefault(l => l.Level == snav.Level);
                    if (dbnav == null)
                    {
                        context.SecurityLevel.Add(snav);
                        context.SaveChanges();
                        dbnav = snav;
                    }
                    userToUpdate.SecurityLevel = dbnav.Id;
                }
                context.User.Update(userToUpdate);
                context.SaveChanges();
                userToUpdate.SecurityLevelNavigation = dbnav;
            }
        }
    }
}
