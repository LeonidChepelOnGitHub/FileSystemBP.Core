using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FileSystemBP.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSystemBP.Core.BusinessLogic
{
    public class FileSystemManager : IFileSystemManager
    {
        public void CreateFile(File parent, File file, Programs[] toOpenWith, Access access, FileType ftype, User user)
        {
            using (var context = new FileSystemContext())
            {
                createFileInternal(context, parent, file, toOpenWith, access, ftype, user);             
            }
        }



        public void CreateFolder(File parent, File folder, Access defaultAccess, User user)
        {
            using (var context = new FileSystemContext())
            {
                var ftype = new FileType
                {
                    Type = "Folder"
                };
                var program = context.Programs.FirstOrDefault(p => p.Name == "Explorer");
                if (program == null)
                {
                    program = new Programs
                    {
                        Name = "Explorer",
                        Description = "Opens folders"
                    };
                    context.Programs.Add(program);
                    context.SaveChanges();
                }
                folder.ProgramDefault = program.Id;
                var toOpenWith = new Programs[] { program };
                createFileInternal(context, parent, folder, toOpenWith, defaultAccess, ftype, user);
            }
        }

        public void DeleteFileOrFolder(int id, User user)
        {
            using (var context = new FileSystemContext())
            {
                var children = context.FileSystem.Where(f => f.ParentId == id);
                var fid = context.FileSystem.FirstOrDefault(f => f.FileId == id);
                if (fid != null)
                {
                    context.FileSystem.Remove(fid);
                    context.SaveChanges();
                }
                var fur = context.FileUserRights.FirstOrDefault(f => f.File == id);
                context.FileUserRights.Remove(fur);
                context.SaveChanges();
                var pftf = context.ProgramFileTypeFile.FirstOrDefault(f => f.File == id);
                context.ProgramFileTypeFile.Remove(pftf);
                context.SaveChanges();
                var file = context.File.FirstOrDefault(f => f.Id == id);
                context.FileSystem.RemoveRange(children);
                context.File.Remove(file);
                context.SaveChanges();
                foreach(var c in children)
                {
                    DeleteFileOrFolder(c.FileId, user);
                }
            }
        }

        public FileType[] GetAllFileTypes()
        {
            using(var context = new FileSystemContext())
            {
                var filetypes = context.FileType.ToArray();
                return filetypes;
            }
        }

        public File GetFileById(int id, User user)
        {
            using (var context = new FileSystemContext())
            {
                var file = context.File.FirstOrDefault(f => f.Id == id);
                return file;
            }
        }

        public File GetFileByPath(string path, User user)
        {
            using (var context = new FileSystemContext())
            {
                var files = path.Split('/');
                var root = context.File.FirstOrDefault(f => f.Name == "/");
                for(int i = 1; i < files.Length; i++)
                {
                    if (root == null)
                    {
                        break;
                    }

                    var allFiles = context
                                        .FileSystem
                                        .Where(f => f.ParentId == root.Id)
                                        .Select(a => a.FileId);
                    var currentFile = context
                                        .File
                                        .Where(f => allFiles.Any(a => a == f.Id))
                                        .FirstOrDefault(file => file.Name == files[i]);


                    root = currentFile;
                    
                }
                return root;
            }
        }

        public string GetPathToFile(File file)
        {
            throw new NotImplementedException();
        }

        public void ModifyFileOrFolder(File toModify, Access access, FileType ftype, User user)
        {
            throw new NotImplementedException();
        }

        public File[] SearchByPredicates<T1, T2>(Expression<Func<bool, T1>> filePred1, Expression<Func<bool, T2>> filePred2, Access access, FileType fileType, Programs program, User user)
        {
            throw new NotImplementedException();
        }


        private void createFileInternal(FileSystemContext context, File parent, File file, Programs[] toOpenWith, Access access, FileType ftype, User user)
        {
            // parent file block----------------------
            if (parent == null)
            {
                parent = context.File.FirstOrDefault(f => f.Name == "/");
                if (parent == null)
                {
                    parent = new File
                    {
                        Name = "/",
                        Size = 12,
                        CreateDate = DateTime.Now
                    };
                    context.File.Add(parent);
                    context.SaveChanges();
                }
            }

            // access block ----------------------------
            var accessFile = context.Access.FirstOrDefault(a => a.Read == access.Read
                                                            && a.Write == access.Write
                                                            && a.Create == access.Create
                                                            && a.Delete == access.Delete);
            if (access == null)
            {
                context.Access.Add(access);
                accessFile = access;
                context.SaveChanges();
            }

            //file type block -------------------------------

            var filetype = context.FileType.FirstOrDefault(ft => ft.Type == ftype.Type);

            if (filetype == null)
            {
                context.FileType.Add(ftype);
                filetype = ftype;
                context.SaveChanges();
            }

            // adding new file
            context.File.Add(file);
            context.SaveChanges();
            context.FileUserRights.Add(new FileUserRights { File = file.Id, User = user.Id, Rights = accessFile.Id });
            context.SaveChanges();
            context.FileSystem.Add(new FileSystem { FileId = file.Id, ParentId = parent.Id });
            foreach (var p in toOpenWith)
            {
                context.ProgramFileTypeFile.Add(
                    new ProgramFileTypeFile
                    {
                        File = file.Id,
                        FileType = filetype.Id,
                        Program = p.Id
                    });
                context.SaveChanges();
            }
           
        }
    }
}
