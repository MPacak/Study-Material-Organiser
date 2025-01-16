using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Models
{
    public partial class StudymaterialorganiserContext : DbContext
    {
        public StudymaterialorganiserContext()
        {
        }

        public StudymaterialorganiserContext(DbContextOptions<StudymaterialorganiserContext> options)
            : base(options)
        {
        }

        public virtual DbSet<StudyGroup> StudyGroup { get; set; } = null!;
        public virtual DbSet<Log> Logs { get; set; } = null!;
        public virtual DbSet<Material> Materials { get; set; } = null!;
        public virtual DbSet<MaterialTag> MaterialTags { get; set; } = null!;
        public virtual DbSet<MaterialUser> MaterialUsers { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserGroup> UserGroups { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=65.21.144.4\\\\SQLEXPRESS,1524;Database=STUDYMATERIALORGANISER;User=sa;Password=RisekNajboljiKolega23;TrustServerCertificate=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudyGroup>(entity =>
            {
                entity.ToTable("Group");

                entity.HasIndex(e => e.Name, "UQ_Group_Name")
                    .IsUnique();

                entity.Property(e => e.Name).HasMaxLength(50);

                
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("Log");

                entity.Property(e => e.Level).HasMaxLength(50);

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(e => e.Idmaterial);

                entity.ToTable("Material");

                entity.Property(e => e.Idmaterial).HasColumnName("IDMaterial");

                entity.Property(e => e.FilePath).HasMaxLength(150);

                entity.Property(e => e.FolderTypeId).HasColumnName("FolderTypeID");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<MaterialTag>(entity =>
            {
                entity.HasKey(e => e.IdmaterialTag);

                entity.ToTable("MaterialTag");

                entity.Property(e => e.IdmaterialTag).HasColumnName("IDMaterialTag");

                entity.Property(e => e.MaterialId).HasColumnName("MaterialID");

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.MaterialTags)
                    .HasForeignKey(d => d.MaterialId)
                    .HasConstraintName("FK_Material");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.MaterialTags)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK_Tag");
            });

            modelBuilder.Entity<MaterialUser>(entity =>
            {
                entity.HasKey(e => e.IdmaterialUser)
                    .HasName("PK__Material__D43459571BF680FD");

                entity.ToTable("MaterialUser");

                entity.Property(e => e.IdmaterialUser).HasColumnName("IDMaterialUser");

                entity.Property(e => e.MaterialId).HasColumnName("MaterialID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.MaterialUsers)
                    .HasForeignKey(d => d.MaterialId)
                    .HasConstraintName("FK__MaterialU__Mater__4E88ABD4");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MaterialUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__MaterialU__UserI__4F7CD00D");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Idtag);

                entity.ToTable("Tag");

                entity.Property(e => e.Idtag).HasColumnName("IDTag");

                entity.Property(e => e.TagName).HasMaxLength(150);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "UQ_User_Email")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "UQ_User_Username")
                    .IsUnique();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(256);

                entity.Property(e => e.LastName).HasMaxLength(256);

                entity.Property(e => e.Phone).HasMaxLength(256);

                entity.Property(e => e.PwdHash).HasMaxLength(256);

                entity.Property(e => e.PwdSalt).HasMaxLength(256);

                entity.Property(e => e.SecurityToken).HasMaxLength(256);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.ToTable("UserGroup");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.UserGroups)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_UserGroup_Group");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserGroups)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserGroup_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
