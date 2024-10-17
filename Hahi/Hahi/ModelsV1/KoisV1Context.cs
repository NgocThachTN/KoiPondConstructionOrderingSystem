using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hahi.ModelsV1
{
    public partial class KoisV1Context : DbContext
    {
        public KoisV1Context()
        {
        }

        public KoisV1Context(DbContextOptions<KoisV1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<ConstructionType> ConstructionTypes { get; set; } = null!;
        public virtual DbSet<Contract> Contracts { get; set; } = null!;
        public virtual DbSet<Design> Designs { get; set; } = null!;
        public virtual DbSet<Maintenance> Maintenances { get; set; } = null!;
        public virtual DbSet<MaintenanceRequest> MaintenanceRequests { get; set; } = null!;
        public virtual DbSet<Request> Requests { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Sample> Samples { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(local);Uid=sa;Pwd=12345;Database=KoisV1;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ConstructionType>(entity =>
            {
                entity.ToTable("ConstructionType");

                entity.Property(e => e.ConstructionTypeId).HasColumnName("ConstructionTypeID");

                entity.Property(e => e.ConstructionTypeName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.ToTable("Contract");

                entity.HasIndex(e => e.RequestId, "UQ__Contract__33A8519B6561141D")
                    .IsUnique();

                entity.Property(e => e.ContractId).HasColumnName("ContractID");

                entity.Property(e => e.ContractEndDate).HasColumnType("datetime");

                entity.Property(e => e.ContractName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ContractStartDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RequestId).HasColumnName("RequestID");

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Request)
                    .WithOne(p => p.Contract)
                    .HasForeignKey<Contract>(d => d.RequestId)
                    .HasConstraintName("FK__Contract__Reques__267ABA7A");
            });

            modelBuilder.Entity<Design>(entity =>
            {
                entity.ToTable("Design");

                entity.Property(e => e.DesignId).HasColumnName("DesignID");

                entity.Property(e => e.ConstructionTypeId).HasColumnName("ConstructionTypeID");

                entity.Property(e => e.DesignImage).IsUnicode(false);

                entity.Property(e => e.DesignName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DesignSize)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.ConstructionType)
                    .WithMany(p => p.Designs)
                    .HasForeignKey(d => d.ConstructionTypeId)
                    .HasConstraintName("FK__Design__Construc__1DE57479");
            });

            modelBuilder.Entity<Maintenance>(entity =>
            {
                entity.ToTable("Maintenance");

                entity.Property(e => e.MaintenanceId).HasColumnName("MaintenanceID");

                entity.Property(e => e.MaintencaceName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MaintenanceRequest>(entity =>
            {
                entity.HasKey(e => new { e.MaintenanceRequestId, e.RequestId })
                    .HasName("PK__Maintena__F3E230DB0DE78748");

                entity.ToTable("Maintenance_Request");

                entity.Property(e => e.MaintenanceRequestId).HasColumnName("Maintenance_RequestID");

                entity.Property(e => e.RequestId).HasColumnName("RequestID");

                entity.Property(e => e.MaintenanceRequestEndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Maintenance_RequestEndDate");

                entity.Property(e => e.MaintenanceRequestStartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Maintenance_RequestStartDate");

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.MaintenanceRequestNavigation)
                    .WithMany(p => p.MaintenanceRequests)
                    .HasForeignKey(d => d.MaintenanceRequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Maintenan__Maint__2B3F6F97");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.MaintenanceRequests)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Maintenan__Reque__2C3393D0");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("Request");

                entity.Property(e => e.RequestId).HasColumnName("RequestID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DesignId).HasColumnName("DesignID");

                entity.Property(e => e.RequestName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SampleId).HasColumnName("SampleID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Design)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.DesignId)
                    .HasConstraintName("FK_Request_Design");

                entity.HasOne(d => d.Sample)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.SampleId)
                    .HasConstraintName("FK_Request_Sample");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Request__UserID__20C1E124");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Sample>(entity =>
            {
                entity.ToTable("Sample");

                entity.Property(e => e.SampleId).HasColumnName("SampleID");

                entity.Property(e => e.ConstructionTypeId).HasColumnName("ConstructionTypeID");

                entity.Property(e => e.SampleImage).IsUnicode(false);

                entity.Property(e => e.SampleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SampleSize)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.ConstructionType)
                    .WithMany(p => p.Samples)
                    .HasForeignKey(d => d.ConstructionTypeId)
                    .HasConstraintName("FK__Sample__Construc__1B0907CE");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.AccountId, "UQ__Users__349DA587BDFDA612")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.AccountId)
                    .HasConstraintName("FK__Users__AccountID__15502E78");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Users__RoleID__164452B1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
