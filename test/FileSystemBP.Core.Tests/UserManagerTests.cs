using FileSystemBP.Core.BusinessLogic;
using FileSystemBP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Tests
{
    public class UserManagerTests
    {
        IUserManager _manager;
        IUserManager _manager2;
        User _user,_user2;
        
        public UserManagerTests()
        {
            _manager = new UserManager();
            _manager2 = new UserManager();

            _user = new User
            {
                Name = Guid.NewGuid().ToString(),
                Login = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString(),

            };
            _user2 = new User
            {
                Name = Guid.NewGuid().ToString(),
                Login = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString(),

            };

        }

        ~UserManagerTests()
        {
            
        }


       [Fact]
       public void Add_ShouldAddNewUser()
        {
            //arrange

            //act
            _manager.Add(_user);
            var actual = _manager2.GetById(_user.Id);
            //assert
            Assert.Equal(_user.Id, actual.Id);
            //clean
            _manager2.Delete(_user.Id);
        }


        [Fact]
        public void Add_ShouldAddNewUserWithSecurityLevel()
        {
            //arrange

            //act
            _user.SecurityLevelNavigation = new SecurityLevel
            {
                Level = "Root"
            };
            _manager.Add(_user);
            var actual = _manager2.GetById(_user.Id);
            //assert
            Assert.Equal("root", actual.SecurityLevelNavigation.Level.ToLower());
            //clean
            _manager2.Delete(_user.Id);
        }

        [Fact]
        public void Delete_ShouldDeleteUser()
        {
            //arrange
            _manager.Add(_user);
            //act
            _manager2.Delete(_user.Id);
            var actual = _manager2.GetById(_user.Id);
            //assert
            Assert.Null(actual);
            //clean
            
        }

        [Fact]
        public void Update_ShouldUpdateUser()
        {
            //arrange
            _manager.Add(_user);
            _user.Name = Guid.NewGuid().ToString();
            //act
            _manager.Update(_user);
            var actual = _manager2.GetById(_user.Id);
            //assert
            Assert.Equal(_user.Name, actual.Name);
            //clean
            _manager2.Delete(_user.Id);
        }


        [Fact]
        public void Update_ShouldUpdateUserWithSecurityLevel()
        {
            //arrange
            _user.SecurityLevelNavigation = new SecurityLevel
            {
                Level = "Root"
            };
            _manager.Add(_user);
            _user.SecurityLevelNavigation = new SecurityLevel
            {
                Level = "RootV2"
            };
            //act
            _manager.Update(_user);
            var actual = _manager2.GetById(_user.Id);
            //assert
            Assert.Equal("rootv2", actual.SecurityLevelNavigation.Level.ToLower());
            //clean
            _manager2.Delete(_user.Id);
        }

        [Fact]
        public void SearchByPredicates_ShouldSearchByPredicates()
        {
            //arrange
            var level = "test";
            _user.SecurityLevelNavigation = new SecurityLevel
            {
                Level = level
            };
            _manager.Add(_user);
            _manager.Add(_user2);

            //act

            var bySecLevel = _manager2.SearchByPredicates(u => u.SecurityLevelNavigation.Level == level, null);
            var byLoginAndPassword = _manager2.SearchByPredicates(u => u.Password == _user2.Password, u => u.Login == _user2.Login);
            //assert
            Assert.Equal(1, bySecLevel.Length);
            Assert.Equal(1, byLoginAndPassword.Length);
            //clean
            _manager2.Delete(_user.Id);
            _manager2.Delete(_user2.Id);
        }

    }
}
