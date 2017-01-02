using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FileSystemBP.Core.Models
{
    public partial class FileSystemContext : DbContext
    {
        public virtual DbSet<Access> Access { get; set; }
        public virtual DbSet<Actions> Actions { get; set; }
        public virtual DbSet<BusinessProcess> BusinessProcess { get; set; }
        public virtual DbSet<BusinessProcessHistory> BusinessProcessHistory { get; set; }
        public virtual DbSet<BusinessProcessPrograms> BusinessProcessPrograms { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<File> File { get; set; }
        public virtual DbSet<FileSystem> FileSystem { get; set; }
        public virtual DbSet<FileType> FileType { get; set; }
        public virtual DbSet<FileUserRights> FileUserRights { get; set; }
        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<ProcessType> ProcessType { get; set; }
        public virtual DbSet<ProgramFileTypeFile> ProgramFileTypeFile { get; set; }
        public virtual DbSet<Programs> Programs { get; set; }
        public virtual DbSet<SecurityLevel> SecurityLevel { get; set; }
        public virtual DbSet<User> User { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=FileSystem;Trusted_Connection=True;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Access>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Actions>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasColumnType("varchar(500)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<BusinessProcess>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.LastRun).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.BusinessProcess)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__BusinessPr__Type__3D5E1FD2");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.BusinessProcess)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__BusinessPr__User__3E52440B");
            });

            modelBuilder.Entity<BusinessProcessHistory>(entity =>
            {
                entity.HasKey(e => new { e.History, e.BusinessProcess })
                    .HasName("PK__Business__13706299C7AB36F7");

                entity.HasOne(d => d.BusinessProcessNavigation)
                    .WithMany(p => p.BusinessProcessHistory)
                    .HasForeignKey(d => d.BusinessProcess)
                    .HasConstraintName("FK__BusinessP__Busin__4D94879B");

                entity.HasOne(d => d.HistoryNavigation)
                    .WithMany(p => p.BusinessProcessHistory)
                    .HasForeignKey(d => d.History)
                    .HasConstraintName("FK__BusinessP__Histo__4CA06362");
            });

            modelBuilder.Entity<BusinessProcessPrograms>(entity =>
            {
                entity.HasKey(e => new { e.Program, e.BusinessProcess })
                    .HasName("PK__Business__C7A5B75FA828D5F7");

                entity.HasOne(d => d.BusinessProcessNavigation)
                    .WithMany(p => p.BusinessProcessPrograms)
                    .HasForeignKey(d => d.BusinessProcess)
                    .HasConstraintName("FK__BusinessP__Busin__49C3F6B7");

                entity.HasOne(d => d.ProgramNavigation)
                    .WithMany(p => p.BusinessProcessPrograms)
                    .HasForeignKey(d => d.Program)
                    .HasConstraintName("FK__BusinessP__Progr__48CFD27E");
            });

            modelBuilder.Entity<Events>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.ActionNavigation)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.Action)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Events__Action__34C8D9D1");

                entity.HasOne(d => d.FileNavigation)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.File)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Events__File__32E0915F");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Events__User__33D4B598");
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("File_FileName");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.File)
                    .HasForeignKey(d => d.Owner)
                    .HasConstraintName("FK__File__Owner__2E1BDC42");

                entity.HasOne(d => d.ProgramDefaultNavigation)
                    .WithMany(p => p.File)
                    .HasForeignKey(d => d.ProgramDefault)
                    .HasConstraintName("FK__File__ProgramDef__2D27B809");
            });

            modelBuilder.Entity<FileSystem>(entity =>
            {
                entity.HasKey(e => new { e.FileId, e.ParentId })
                    .HasName("PK__FileSyst__823C0DA98BB41972");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.FileSystemFile)
                    .HasForeignKey(d => d.FileId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__FileSyste__FileI__37A5467C");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.FileSystemParent)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__FileSyste__Paren__38996AB5");
            });

            modelBuilder.Entity<FileType>(entity =>
            {
                entity.HasIndex(e => e.Type)
                    .HasName("UQ__FileType__F9B8A48B21878035")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<FileUserRights>(entity =>
            {
                entity.HasKey(e => new { e.File, e.User, e.Rights })
                    .HasName("PK__FileUser__EDD36B047C4B62A8");

                entity.HasOne(d => d.FileNavigation)
                    .WithMany(p => p.FileUserRights)
                    .HasForeignKey(d => d.File)
                    .HasConstraintName("FK__FileUserRi__File__5070F446");

                entity.HasOne(d => d.RightsNavigation)
                    .WithMany(p => p.FileUserRights)
                    .HasForeignKey(d => d.Rights)
                    .HasConstraintName("FK__FileUserR__Right__52593CB8");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.FileUserRights)
                    .HasForeignKey(d => d.User)
                    .HasConstraintName("FK__FileUserRi__User__5165187F");
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.ModifiedNavigation)
                    .WithMany(p => p.History)
                    .HasForeignKey(d => d.Modified)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__History__Modifie__412EB0B6");
            });

            modelBuilder.Entity<ProcessType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<ProgramFileTypeFile>(entity =>
            {
                entity.HasKey(e => new { e.File, e.FileType, e.Program })
                    .HasName("PK__ProgramF__3EE47F7623802D0D");

                entity.HasOne(d => d.FileNavigation)
                    .WithMany(p => p.ProgramFileTypeFile)
                    .HasForeignKey(d => d.File)
                    .HasConstraintName("FK__ProgramFil__File__5535A963");

                entity.HasOne(d => d.FileTypeNavigation)
                    .WithMany(p => p.ProgramFileTypeFile)
                    .HasForeignKey(d => d.FileType)
                    .HasConstraintName("FK__ProgramFi__FileT__5629CD9C");

                entity.HasOne(d => d.ProgramNavigation)
                    .WithMany(p => p.ProgramFileTypeFile)
                    .HasForeignKey(d => d.Program)
                    .HasConstraintName("FK__ProgramFi__Progr__571DF1D5");
            });

            modelBuilder.Entity<Programs>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Programs__737584F6A9330A00")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasColumnType("varchar(500)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<SecurityLevel>(entity =>
            {
                entity.HasIndex(e => e.Level)
                    .HasName("UQ__Security__AAF89962D2C96412")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasColumnType("varchar(15)");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("User_UserName");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.SecurityLevelNavigation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.SecurityLevel)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__User__SecurityLe__2A4B4B5E");
            });
        }
    }
}