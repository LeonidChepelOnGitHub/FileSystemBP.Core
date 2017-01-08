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
                foreach (var c in children)
                {
                    DeleteFileOrFolder(c.FileId, user);
                }
            }
        }

        public FileType[] GetAllFileTypes()
        {
            using (var context = new FileSystemContext())
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
                for (int i = 1; i < files.Length; i++)
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

        public FileType GetFileType(int fileId, User user)
        {
            using (var context = new FileSystemContext())
            {
                var ftypeId = context
                                .ProgramFileTypeFile
                                .FirstOrDefault(f => f.File == fileId)
                                .FileType;
                var ft = context.FileType.FirstOrDefault(f => f.Id == ftypeId);
                return ft;
            }
        }

        public string GetPathToFile(File file)
        {
            using (var context = new FileSystemContext())
            {
                if (file.Name == "/") return "/";
                var f = file;
                var path = "";
                var parent = new FileSystem();
                do
                {
                    path = "/" + f.Name + path;

                    parent = context
                                    .FileSystem
                                    .FirstOrDefault(fs => fs.FileId == f.Id);
                    if (parent != null)
                    {
                        f = context.File.FirstOrDefault(fl => fl.Id == parent.ParentId);
                    }

                } while (parent != null);
                return path.Substring(2);
            }
        }

        public Access GetUserAccess(int fileId, User user)
        {
            using (var context = new FileSystemContext())
            {
                var aid = context
                              .FileUserRights
                              .FirstOrDefault(f => f.File == fileId)
                              .Rights;
                var access = context.Access.FirstOrDefault(f => f.Id == aid);
                return access;
            }
        }

        public void ModifyFileOrFolder(File toModify, Access access, FileType ftype, User user)
        {
            using (var context = new FileSystemContext())
            {

                if (ftype != null)
                {
                    var ftid = context.FileType.FirstOrDefault(ft => ft.Type == ftype.Type);
                    if (ftid == null)
                    {
                        ftid = ftype;
                        context.FileType.Add(ftid);
                        context.SaveChanges();
                    }
                    var ftypes = context
                                    .ProgramFileTypeFile
                                    .Where(t => t.File == toModify.Id).ToArray();
                    foreach (var t in ftypes)
                    {
                        context.ProgramFileTypeFile.Remove(t);
                        context.SaveChanges();
                        t.FileType = ftid.Id;
                        context.ProgramFileTypeFile.Add(t);
                        context.SaveChanges();
                    }
                }

                if (user != null && access != null)
                {
                    var aid = context.Access
                                    .FirstOrDefault(t => t.Delete == access.Delete
                                                    && t.Create == access.Create
                                                    && t.Read == access.Read
                                                    && t.Write == access.Write);
                    if (aid == null)
                    {
                        aid = access;
                        context.Access.Add(aid);
                        context.SaveChanges();
                    }
                    var fur = context
                                  .FileUserRights
                                  .FirstOrDefault(t => t.File == toModify.Id
                                                        && t.User == user.Id);
                    context.FileUserRights.Remove(fur);
                    context.SaveChanges();
                    fur.Rights = aid.Id;
                    context.FileUserRights.Add(fur);
                    context.SaveChanges();
                }

                context.File.Update(toModify);
                context.SaveChanges();



            }

        }

        public File[] SearchByPredicates(Expression<Func<File, bool>> filePred1, Expression<Func<File, bool>> filePred2, Access access, FileType fileType, Programs program, User user)
        {
            using (var context = new FileSystemContext())
            {
                var fur = context.FileUserRights.AsQueryable();
                if (user != null)
                {
                    fur = fur.Where(t => t.User == user.Id);
                }
                if (access != null)
                {
                    fur = fur.Where(t => t.Rights == access.Id);
                }
                var pftf = context.ProgramFileTypeFile.AsQueryable();
                if (fileType != null)
                {
                    pftf = pftf.Where(t => t.FileType == fileType.Id);
                }
                if (program != null)
                {
                    pftf = pftf.Where(t => t.Program == program.Id);
                }
                var join = fur.Join(pftf, t1 => t1.File, t2 => t2.File, (t1, t2) => t1.File);
                var files = context.File.AsQueryable();
                var finalJoin = join.Join(files, t1 => t1, t2 => t2.Id, (t1, t2) => t2);

                if (filePred1 != null)
                {
                    finalJoin = finalJoin.Where(filePred1);
                }
                if (filePred2 != null)
                {
                    finalJoin = finalJoin.Where(filePred2);
                }

                return finalJoin.ToArray();
            }
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
            if (accessFile == null)
            {
                context.Access.Add(access);
                context.SaveChanges();
            }else
            {
                access.Id = accessFile.Id;
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
