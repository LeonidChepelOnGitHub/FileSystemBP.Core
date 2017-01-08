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
        Programs _program;
        User _user;
        Access _access;
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
            _user = new User
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
                context.User.Add(_user);
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
           
            var filetypesLength = _manager.GetAllFileTypes().Length;
            //act
            _manager.CreateFolder(null, _file, _access, _user);
            _manager.CreateFile(_file, _file2, new Programs[] { _program }, _access, _filetype2, _user);
            var createdFile = _manager2.GetFileByPath('/' + _file.Name + '/' + _file2.Name, _user);
            var filetypes2Length = _manager2.GetAllFileTypes().Length;
            //assert
            Assert.NotNull(createdFile);
            Assert.Equal(filetypesLength + 1, filetypes2Length);
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

    }
}
