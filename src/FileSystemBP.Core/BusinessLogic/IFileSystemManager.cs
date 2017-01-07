using FileSystemBP.Core.Models;
using System;
using System.Linq.Expressions;

namespace FileSystemBP.Core.BusinessLogic
{
    public interface IFileSystemManager
    {
        File GetFileByPath(string path);
        File GetFileById(int id);
        File[] SearchByPredicates<T1, T2>(Expression<Func<bool, T1>> filePred1,
                                              Expression<Func<bool, T2>> filePred2,
                                              Access access, FileType fileType,
                                              Program program, User user);
        void CreateFile(File file, Program toOpenWith, Access access, FileType ftype);
        void CreateFolder(File parent ,Access defaultAccess);
        void DeleteFileOrFolder(int id);
        void ModifyFileOrFolder(File toModify, Access access, FileType ftype);
        string GetPathToFile(File file);
        FileType[] GetAllFileTypes();

    }
}
