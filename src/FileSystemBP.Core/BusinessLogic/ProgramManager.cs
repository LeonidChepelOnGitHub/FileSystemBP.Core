using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FileSystemBP.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSystemBP.Core.BusinessLogic
{
    public class ProgramManager : IProgramManager
    {
        public void Add(Programs program, FileType filetype)
        {
            using(var context = new FileSystemContext())
            {
             
                var filesCanOpen = context
                                    .ProgramFileTypeFile
                                    .Where(ftf => ftf.FileTypeNavigation.Type == filetype.Type)
                                    .Select(f=>f.File)
                                    .ToArray();
                addInternal(context, program, filetype, filesCanOpen);
            }
        }

        private void addInternal(FileSystemContext context, Programs program, FileType filetype, int[] fileIds)
        {
            
                var ft = context.FileType.FirstOrDefault(f => f.Type == filetype.Type);
                if (ft == null)
                {
                    throw new PlatformNotSupportedException("No filetype specified for current program");
                }
                context.Programs.Add(program);
               
                foreach (var f in fileIds)
                {
                    context
                        .ProgramFileTypeFile
                        .Add(new ProgramFileTypeFile
                        {
                            File = f,
                            FileType = ft.Id,
                            Program = program.Id
                        });
                }
                context.SaveChanges();
           
        }

        public void Add(Programs program, FileType filetype, int[] fileIds)
        {
            using (var context = new FileSystemContext())
            {
                addInternal(context, program, filetype, fileIds);
            }
        }

        public void Delete(int id)
        {
            using(var context = new FileSystemContext())
            {
                var range = context.ProgramFileTypeFile.Where(p => p.Program == id);
                context.ProgramFileTypeFile.RemoveRange(range);
                context.Programs.Remove(
                    context.Programs.FirstOrDefault(p => p.Id == id));
                context.SaveChanges();

            }
        }

        public Programs GetById(int id)
        {
            using(var context = new FileSystemContext())
            {
                return context.Programs.Include(p=>p.ProgramFileTypeFile).FirstOrDefault(p => p.Id == id);
            }
        }

        public Programs[] SearchByFileType(FileType filetype)
        {
            using(var context = new FileSystemContext())
            {
                var programs = context
                                .ProgramFileTypeFile
                                .Where(p => p.FileTypeNavigation.Type == filetype.Type)
                                .Select(p => p.ProgramNavigation)
                                .ToArray();
                return programs;
            }
        }

        public Programs[] SearchByPredicate(Expression<Func<Programs, bool>> pred)
        {
            using (var context = new FileSystemContext())
            {
                var programs = context.Programs.Where(pred).ToArray();
                return programs;
            }
        }

        public void Update(Programs programToUpdate)
        {
            using(var context = new FileSystemContext())
            {
                context.Programs.Update(programToUpdate);
                context.SaveChanges();
            }
        }
    }
}
