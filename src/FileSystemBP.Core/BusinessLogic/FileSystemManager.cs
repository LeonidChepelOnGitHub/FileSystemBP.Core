using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FileSystemBP.Core.Models;

namespace FileSystemBP.Core.BusinessLogic
{
    public class FileSystemManager : IFileSystemManager
    {
        public void CreateFile(File file, Program toOpenWith, Access access, FileType ftype)
        {
            throw new NotImplementedException();
        }

        public void CreateFolder(File parent, Access defaultAccess)
        {
            throw new NotImplementedException();
        }

        public void DeleteFileOrFolder(int id)
        {
            throw new NotImplementedException();
        }

        public FileType[] GetAllFileTypes()
        {
            throw new NotImplementedException();
        }

        public File GetFileById(int id)
        {
            throw new NotImplementedException();
        }

        public File GetFileByPath(string path)
        {
            throw new NotImplementedException();
        }

        public string GetPathToFile(File file)
        {
            throw new NotImplementedException();
        }

        public void ModifyFileOrFolder(File toModify, Access access, FileType ftype)
        {
            throw new NotImplementedException();
        }

        public File[] SearchByPredicates<T1, T2>(Expression<Func<bool, T1>> filePred1, Expression<Func<bool, T2>> filePred2, Access access, FileType fileType, Program program, User user)
        {
            throw new NotImplementedException();
        }
    }
}
