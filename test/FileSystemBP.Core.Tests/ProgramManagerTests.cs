using FileSystemBP.Core.BusinessLogic;
using FileSystemBP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Tests
{
    public class ProgramManagerTests
    {
        IProgramManager _manager;
        IProgramManager _manager2;
        File _file;
        FileType _filetype, _filetype2;
        public ProgramManagerTests()
        {
            _manager = new ProgramManager();
            _manager2 = new ProgramManager();

            _file = new File
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

            using (var context = new FileSystemContext())
            {
                context.File.Add(_file);
                context.FileType.Add(_filetype);
                context.FileType.Add(_filetype2);
                context.SaveChanges();
            }

        }

        ~ProgramManagerTests()
        {
            using (var context = new FileSystemContext())
            {
                context.File.Remove(_file);
                context.FileType.Remove(_filetype);
                context.FileType.Remove(_filetype2);
                context.SaveChanges();
            }
        }


        [Fact]
        public void Add_ShouldAddNewProgram()
        {
            //arrange
            var program = new Programs
            {
                Name = Guid.NewGuid().ToString(),
                Description = "test",
            };
            //act
            _manager.Add(program,_filetype, new int[] { _file.Id });
            var program2 = _manager2.GetById(program.Id);
            //assert
            Assert.Equal(program.Name, program2.Name);
            Assert.True(program2.ProgramFileTypeFile.Any(t => t.FileType == _filetype.Id));
            //clean
            _manager2.Delete(program.Id);
        }


        [Fact]
        public void Delete_ShouldDeleteProgram()
        {
            //arrange
            var program = new Programs
            {
                Name = Guid.NewGuid().ToString(),
                Description = "test",
            };
            //act
            _manager.Add(program, _filetype, new int[] { _file.Id });
            _manager2.Delete(program.Id);
            var program2 = _manager.GetById(program.Id);
            //assert
            Assert.Null(program2);
        }

        [Fact]
        public void Update_ShouldUpdateProgram()
        {
            //arrange
            var program = new Programs
            {
                Name = Guid.NewGuid().ToString(),
                Description = "test",
            };

            var newName = Guid.NewGuid().ToString();
            //act
            _manager.Add(program, _filetype, new int[] { _file.Id });
            program.Name = newName;
            program.ProgramFileTypeFile.Add(new ProgramFileTypeFile { FileType = _filetype2.Id, File=_file.Id,Program=program.Id });
            _manager.Update(program);
            var program2 = _manager2.GetById(program.Id);
            //assert
            Assert.Equal(program.Name, program2.Name);
            Assert.True(program.ProgramFileTypeFile.Any(t => t.FileType == _filetype2.Id));
            //clean
            _manager2.Delete(program.Id);
        }



        [Fact]
        public void Search_ShouldSearchProgram()
        {
            //arrange
            var program1 = new Programs
            {
                Name = Guid.NewGuid().ToString(),
                Description = "test",
            };

            var program2 = new Programs
            {
                Name = Guid.NewGuid().ToString(),
                Description = "test",
            };
            Expression<Func<Programs, bool>> searchByName = t => t.Name == program1.Name;
             //act
            _manager.Add(program1, _filetype, new int[] { _file.Id });
            _manager.Add(program2, _filetype2, new int[] { _file.Id });
            var resultByName = _manager2.SearchByPredicate(searchByName);
            var resultByFileType = _manager2.SearchByFileType(_filetype2);
            //assert
            Assert.Equal(1, resultByName.Length);
            Assert.Equal(1, resultByFileType.Length);
            Assert.Equal(program1.Name, resultByName[0].Name);
            Assert.Equal(program2.Id, resultByFileType[0].Id);
             //clean
            _manager2.Delete(program1.Id);
            _manager2.Delete(program2.Id);
        }

    }
}
