using FileSystemBP.Core.BusinessLogic;
using FileSystemBP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Tests
{
    public class FileSystemManagerTests
    {
        IFileSystemManager _manager;
        IFileSystemManager _manager2;
        File _file,_file2;
        FileType _filetype, _filetype2;
        Programs _program,_program2;
        User _user,_user2;
        Access _access,_access2;
        public FileSystemManagerTests()
        {
            _manager = new FileSystemManager();
            _manager2 = new FileSystemManager();
            _access = new Access
            {
                Create = true,
                Delete = true,
                Read = true,
                Write = true
            };
            _access2 = new Access
            {
                Create = true,
                Delete = false,
                Read = true,
                Write = true
            };
            _file = new File
            {
                Name = Guid.NewGuid().ToString(),
                Size = 12,
                CreateDate = DateTime.Now
            };
            _file2 = new File
            {
                Name = Guid.NewGuid().ToString(),
                Size = 12,
                CreateDate = DateTime.Now
            };
            _filetype = new FileType
            {
                Type = Guid.NewGuid().ToString()
            };

            _filetype2 = new FileType
            {
                Type = Guid.NewGuid().ToString()
            };
            _program = new Programs
            {
                Name = Guid.NewGuid().ToString(),
                Description = "test",
            };
            _program2 = new Programs
            {
                Name = Guid.NewGuid().ToString(),
                Description = "test",
            };
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
            using (var context = new FileSystemContext())
            {
                //context.File.Add(_file);
                context.FileType.Add(_filetype);
                context.Programs.Add(_program);
                context.Programs.Add(_program2);
                context.User.Add(_user);
                context.User.Add(_user2);
                //context.FileType.Add(_filetype2);
                context.SaveChanges();
            }

        }

        ~FileSystemManagerTests()
        {
            using (var context = new FileSystemContext())
            {
                //context.File.Remove(_file);
                context.FileType.Remove(_filetype);
                context.User.Remove(_user);
                context.User.Remove(_user2);
                context.Programs.Remove(_program);
                context.Programs.Remove(_program2);
                //context.FileType.Remove(_filetype2);
                context.SaveChanges();
            }
        }


        [Fact]
        public void Add_ShouldAddNewFileWithExistingFileTypeAndProgramToRoot()
        {
            //arrange
            
            //act
            _manager.CreateFile(null, _file, new Programs[] { _program }, _access, _filetype,_user);
            var createdFile = _manager2.GetFileById(_file.Id, _user);
            //assert
            Assert.NotNull(createdFile);
            //clean
            _manager2.DeleteFileOrFolder(_file.Id,_user);
        }

        [Fact]
        public void Add_ShouldAddNewFileWithExistingFileTypeAndProgramToFolder()
        {
            //arrange
           
            //act
            _manager.CreateFolder(null, _file, _access, _user);
            _manager.CreateFile(_file, _file2, new Programs[] { _program }, _access, _filetype, _user);
            var createdFile = _manager2.GetFileByPath('/' + _file.Name + '/' + _file2.Name, _user);
            //assert
            Assert.NotNull(createdFile);
            //clean
            _manager2.DeleteFileOrFolder(_file.Id, _user);
        }

        [Fact]
        public void Add_ShouldAddNewFileWithNewFileType()
        {
            //arrange
            //act
            _manager.CreateFolder(null, _file, _access, _user);
            _manager.CreateFile(_file, _file2, new Programs[] { _program }, _access, _filetype2, _user);
            var createdFile = _manager2.GetFileByPath('/' + _file.Name + '/' + _file2.Name, _user);
            var filetypes = _manager2.GetAllFileTypes();
            //assert
            Assert.True(filetypes.Any(ft => ft.Type == _filetype2.Type));
            //clean
            _manager2.DeleteFileOrFolder(_file.Id, _user);
        }

        [Fact]
        public void DeleteFileOrFolder_ShouldDeleteFolderRecursivelyWithFiles()
        {
            //arrange
            _manager.CreateFolder(null, _file, _access, _user);
            _manager.CreateFile(_file, _file2, new Programs[] { _program }, _access, _filetype, _user);

            //act
            _manager2.DeleteFileOrFolder(_file.Id, _user);
            var createdFile = _manager2.GetFileByPath('/' + _file.Name + '/' + _file2.Name, _user);
            var createdFolder = _manager.GetFileById(_file.Id, _user);
            //assert
            Assert.Null(createdFile);
            Assert.Null(createdFolder);
            //clean
            
        }

        [Fact]
        public void DeleteFileOrFolder_ShouldDeleteEmptyFolder()
        {
            //arrange
            _manager.CreateFolder(null, _file, _access, _user);
          

            //act
            _manager2.DeleteFileOrFolder(_file.Id, _user);
            var createdFolder = _manager.GetFileByPath("/"+_file.Name, _user);
            //assert
            
            Assert.Null(createdFolder);
            //clean

        }

        [Fact]
        public void DeleteFileOrFolder_ShouldDeleteFile()
        {
            //arrange
            _manager.CreateFolder(null, _file, _access, _user);
            _manager.CreateFile(_file, _file2, new Programs[] { _program }, _access, _filetype, _user);

            //act
            _manager2.DeleteFileOrFolder(_file2.Id, _user);
            var createdFile = _manager2.GetFileByPath('/' + _file.Name + '/' + _file2.Name, _user);
            var createdFolder = _manager.GetFileById(_file.Id, _user);
            //assert
            Assert.Null(createdFile);
            Assert.NotNull(createdFolder);
            //clean

        }

        [Fact]
        public void GetPathToFile_ShouldReturnPathToFile()
        {
            //arrange
            _manager.CreateFolder(null, _file, _access, _user);
            _manager.CreateFile(_file, _file2, new Programs[] { _program }, _access, _filetype, _user);
            //act
            var path = _manager2.GetPathToFile(_file2);
            //assert
            Assert.Equal("/" + _file.Name + "/" + _file2.Name, path);
            //clean
            _manager2.DeleteFileOrFolder(_file.Id, _user);
        }


        [Fact]
        public void ModifyFileOrFolder_ShouldModifyFilePropAndFolderProp()
        {
            //arrange
            _manager.CreateFolder(null, _file, _access, _user);
            _manager.CreateFile(_file, _file2, new Programs[] { _program }, _access, _filetype, _user);
            var newSize = 120;
            var newName = Guid.NewGuid().ToString();
            //act
            _file.Size = newSize;
            _file2.Name = newName;
            _manager2.ModifyFileOrFolder(_file, null, null, _user);
            _manager2.ModifyFileOrFolder(_file2, null, null, _user);
            var file = _manager.GetFileById(_file.Id,_user);
            var file2 = _manager2.GetFileById(_file2.Id, _user);
            //assert
            Assert.Equal(newSize,file.Size);
            Assert.Equal(newName, file2.Name);
            //clean
            _manager2.DeleteFileOrFolder(_file.Id, _user);
        }


        [Fact]
        public void ModifyFileOrFolder_ShouldModifyFileType()
        {
            //arrange
             _manager.CreateFile(null, _file, new Programs[] { _program }, _access, _filetype, _user);
           
            //act
            _manager2.ModifyFileOrFolder(_file, null, _filetype2, _user);
            var fileType = _manager.GetFileType(_file.Id,_user);
             //assert
            Assert.Equal(_filetype2.Id,fileType.Id);
            //clean
            _manager2.DeleteFileOrFolder(_file.Id, _user);
        }

        [Fact]
        public void ModifyFileOrFolder_ShouldModifyAccess()
        {
            //arrange
            _manager.CreateFile(null, _file, new Programs[] { _program }, _access, _filetype, _user);
            var newAccess = new Access
            {
                Create = false,
                Delete = false,
                Read = true,
                Write = true
            };
            //act

            _manager2.ModifyFileOrFolder(_file, newAccess, null, _user);
            var access = _manager.GetUserAccess(_file.Id,_user);
            //assert
            Assert.NotEqual(_access.Id, access.Id);
            //clean
            _manager2.DeleteFileOrFolder(_file.Id, _user);
        }

        [Fact]
        public void SearchByPredicates_ShouldSearchByFileProp()
        {
            //arrange
            _manager.CreateFile(null, _file, new Programs[] { _program }, _access, _filetype, _user);
            _manager.CreateFile(null, _file2, new Programs[] { _program }, _access, _filetype, _user);
            Expression<Func<File, bool>> f1 = f => f.Name == _file.Name;
            Expression<Func<File, bool>> f2 = f => f.Id == _file2.Id;
            //act
            var s1 = _manager2.SearchByPredicates(f1, null, null, null, null, _user);
            var s2 = _manager2.SearchByPredicates(f2, null, null, null, null, _user);
            var s_none = _manager2.SearchByPredicates(f1, f2, null, null, null, _user);
            //assert
            Assert.Equal(1, s1.Length);
            Assert.Equal(1, s2.Length);
            Assert.Equal(0,s_none.Length);
            //clean
            _manager2.DeleteFileOrFolder(_file.Id, _user);
            _manager2.DeleteFileOrFolder(_file2.Id, _user);
        }


        [Fact]
        public void SearchByPredicates_ShouldSearchByFileType()
        {
            //arrange
            _manager.CreateFile(null, _file, new Programs[] { _program }, _access, _filetype, _user);
            _manager.CreateFile(null, _file2, new Programs[] { _program }, _access, _filetype2, _user);
           
            //act
            var s1 = _manager2.SearchByPredicates(null, null, null, _filetype, null, _user);
            var s2 = _manager2.SearchByPredicates(null, null, null, _filetype2, null, _user);
            
            //assert
            Assert.Equal(1, s1.Length);
            Assert.Equal(1, s2.Length);
            //clean
            _manager2.DeleteFileOrFolder(_file.Id, _user);
            _manager2.DeleteFileOrFolder(_file2.Id, _user);
        }

        [Fact]
        public void SearchByPredicates_ShouldSearchByProgram()
        {
            //arrange
            
            _manager.CreateFile(null, _file, new Programs[] { _program }, _access, _filetype, _user);
            _manager.CreateFile(null, _file2, new Programs[] { _program2 }, _access, _filetype, _user);
           
            //act
            var s1 = _manager2.SearchByPredicates(null, null, null, null, _program, _user);
            var s2 = _manager2.SearchByPredicates(null, null, null, null, _program2, _user);
           
            //assert
            Assert.Equal(1, s1.Length);
            Assert.Equal(1, s2.Length);
           
            //clean
            _manager2.DeleteFileOrFolder(_file.Id, _user);
            _manager2.DeleteFileOrFolder(_file2.Id, _user);
        }

        [Fact]
        public void SearchByPredicates_ShouldSearchByAccess()
        {
            //arrange
            _manager.CreateFile(null, _file, new Programs[] { _program }, _access, _filetype, _user);
            _manager.CreateFile(null, _file2, new Programs[] { _program }, _access2, _filetype, _user);
            
            //act
            var s1 = _manager2.SearchByPredicates(null, null, _access, null, null, _user);
            var s2 = _manager2.SearchByPredicates(null, null, _access2, null, null, _user);
         
            //assert
            Assert.Equal(1, s1.Length);
            Assert.Equal(1, s2.Length);
           
            //clean
            _manager2.DeleteFileOrFolder(_file.Id, _user);
            _manager2.DeleteFileOrFolder(_file2.Id, _user);
        }

       

    }
}
