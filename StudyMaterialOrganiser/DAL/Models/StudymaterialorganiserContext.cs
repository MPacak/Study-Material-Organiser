using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class StudymaterialorganiserContext : DbContext
{
    public StudymaterialorganiserContext()
    {
    }

    public StudymaterialorganiserContext(DbContextOptions<StudymaterialorganiserContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialTag> MaterialTags { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name = ConnectionStrings:StudyOrganiserConnStr");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Idgroup).HasName("PK__Group__CB4260CAA6096359");

            entity.ToTable("Group");

            entity.Property(e => e.Idgroup)
                .ValueGeneratedNever()
                .HasColumnName("IDGroup");
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Tag).WithMany(p => p.Groups)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("FK__Group__TagId__36B12243");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Idlog).HasName("PK__Log__95D002089596F711");

            entity.ToTable("Log");

            entity.Property(e => e.Idlog)
                .ValueGeneratedNever()
                .HasColumnName("IDLog");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Logs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Log__UserId__3D5E1FD2");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Idmaterial).HasName("PK_Material");

            entity.ToTable("Material");

            entity.Property(e => e.Idmaterial).HasColumnName("IDMaterial");
            entity.Property(e => e.FilePath).HasMaxLength(150);
            entity.Property(e => e.FolderTypeId).HasColumnName("FolderTypeID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<MaterialTag>(entity =>
        {
            entity.HasKey(e => e.IdmaterialTag).HasName("PK_MaterialTag");

            entity.ToTable("MaterialTag");

            entity.Property(e => e.IdmaterialTag).HasColumnName("IDMaterialTag");
            entity.Property(e => e.MaterialId).HasColumnName("MaterialID");
            entity.Property(e => e.TagId).HasColumnName("TagID");

            entity.HasOne(d => d.Material).WithMany(p => p.MaterialTags)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK_Material");

            entity.HasOne(d => d.Tag).WithMany(p => p.MaterialTags)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("FK_Tag");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Idrole).HasName("PK__Role__A1BA16C49B058169");

            entity.ToTable("Role");

            entity.Property(e => e.Idrole)
                .ValueGeneratedNever()
                .HasColumnName("IDRole");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Idtag).HasName("PK__Tag__A7023751E266E44B");

            entity.ToTable("Tag");

            entity.Property(e => e.Idtag).HasColumnName("IDTag");
            entity.Property(e => e.TagName).HasMaxLength(150);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PK__User__EAE6D9DFF109D726");

            entity.ToTable("User");

            entity.Property(e => e.Iduser)
                .ValueGeneratedNever()
                .HasColumnName("IDUser");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("Last_Name");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("User_Name");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User__RoleId__398D8EEE");
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.GroupId }).HasName("PK__UserGrou__A6C1637A32F64E20");

            entity.ToTable("UserGroup");

            entity.HasOne(d => d.Group).WithMany(p => p.UserGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserGroup__Group__412EB0B6");

            entity.HasOne(d => d.User).WithMany(p => p.UserGroups)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserGroup__UserI__403A8C7D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
