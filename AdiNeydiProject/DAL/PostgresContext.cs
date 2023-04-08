using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AdiNeydiProject.DAL;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Audio> Audios { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Picture> Pictures { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;Database=adineydidb;User Id=postgres;Password=0000;");
// Server=185.87.253.114;Database=adineydidb;User Id=postgres;Password=12345678;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Audio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Audio_pkey");

            entity.ToTable("Audio");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Path)
                .HasMaxLength(255)
                .IsFixedLength();
            entity.Property(e => e.PostId).HasColumnName("Post_ID");
            entity.Property(e => e.UserId).HasColumnName("User_ID");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Category_pkey");

            entity.ToTable("Category");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsFixedLength();
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Comment_pkey");

            entity.ToTable("Comment");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("Created_Time");
            entity.Property(e => e.IpAddress).HasColumnName("Ip_Address");
            entity.Property(e => e.PostId).HasColumnName("Post_ID");
            entity.Property(e => e.RepliedCommentId).HasColumnName("RepliedComment_ID");
            entity.Property(e => e.TrueComment).HasColumnName("trueComment");
            entity.Property(e => e.UserId).HasColumnName("User_ID");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.PostId).HasColumnName("Post_ID");
            entity.Property(e => e.UserId).HasColumnName("User_ID");
        });

        modelBuilder.Entity<Picture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Picture_pkey");

            entity.ToTable("Picture");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.PostId).HasColumnName("Post_Id");
            entity.Property(e => e.UserId).HasColumnName("User_ID");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Post_pkey");

            entity.ToTable("Post");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.CategoryId).HasColumnName("Category_ID");
            entity.Property(e => e.CreatedTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("Created_Time");
            entity.Property(e => e.IsApproved).HasColumnName("Is_Approved");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsFixedLength();
            entity.Property(e => e.TypeId).HasColumnName("Type_ID");
            entity.Property(e => e.UserId).HasColumnName("User_ID");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Type_pkey");

            entity.ToTable("Type");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsFixedLength();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedTime).HasColumnName("Created_Time");
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .IsFixedLength();
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.IsPhoneVerificated).HasColumnName("Is_Phone_Verificated");
            entity.Property(e => e.LastLogin).HasColumnType("timestamp without time zone");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Password)
                .HasMaxLength(80)
                .IsFixedLength();
            entity.Property(e => e.UserName)
                .HasMaxLength(30)
                .IsFixedLength();
            entity.Property(e => e.UserTypeId).HasColumnName("UserType_ID");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserType_pkey");

            entity.ToTable("UserType");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
