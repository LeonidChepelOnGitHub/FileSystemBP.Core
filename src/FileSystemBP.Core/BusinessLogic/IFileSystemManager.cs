using FileSystemBP.Core.Models;
using System;
using System.Linq.Expressions;

namespace FileSystemBP.Core.BusinessLogic
{
    public interface IFileSystemManager
    {
        File GetFileByPath(string path, User user);
        File GetFileById(int id, User user);
        File[] SearchByPredicates(Expression<Func<File,bool>> filePred1,
                                              Expression<Func<File, bool>> filePred2,
                                              Access access, FileType fileType,
                                              Programs program, User user);
        void CreateFile(File parent, File file, Programs[] toOpenWith, Access access, FileType ftype, User user);
        void CreateFolder(File parent, File folder, Access defaultAccess, User user);
        void DeleteFileOrFolder(int id, User user);
        void ModifyFileOrFolder(File toModify, Access access, FileType ftype, User user);
        string GetPathToFile(File file);
        FileType[] GetAllFileTypes();
        FileType GetFileType(int fileId, User user);
        Access GetUserAccess(int fileId, User user);
        File[] GetAll();

    }
}
