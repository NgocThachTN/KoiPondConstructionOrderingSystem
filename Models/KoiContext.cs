﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KoiPond.Models;

public partial class KoiContext : DbContext
{
    public KoiContext()
    {
    }

    public KoiContext(DbContextOptions<KoiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<ConstructionType> ConstructionTypes { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<Design> Designs { get; set; }

    public virtual DbSet<Maintenance> Maintenances { get; set; }

    public virtual DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sample> Samples { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=koipond.database.windows.net;Initial Catalog=Koi;Persist Security Info=True;User ID=senjougahara;Password=Kingreshlol123;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA586D82D3A1D");

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
            entity.HasKey(e => e.ConstructionTypeId).HasName("PK__Construc__5793AC97F6ED7ECB");

            entity.ToTable("ConstructionType");

            entity.Property(e => e.ConstructionTypeId).HasColumnName("ConstructionTypeID");
            entity.Property(e => e.ConstructionTypeName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PK__Contract__C90D34090EE63CF7");

            entity.ToTable("Contract");

            entity.HasIndex(e => e.RequestId, "UQ__Contract__33A8519BE582FE01").IsUnique();

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

            entity.HasOne(d => d.Request).WithOne(p => p.Contract)
                .HasForeignKey<Contract>(d => d.RequestId)
                .HasConstraintName("FK__Contract__Reques__318258D2");
        });

        modelBuilder.Entity<Design>(entity =>
        {
            entity.HasKey(e => e.DesignId).HasName("PK__Design__32B8E17F5AE0C947");

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

            entity.HasOne(d => d.ConstructionType).WithMany(p => p.Designs)
                .HasForeignKey(d => d.ConstructionTypeId)
                .HasConstraintName("FK__Design__Construc__28ED12D1");
        });

        modelBuilder.Entity<Maintenance>(entity =>
        {
            entity.HasKey(e => e.MaintenanceId).HasName("PK__Maintena__E60542B519AE37CB");

            entity.ToTable("Maintenance");

            entity.Property(e => e.MaintenanceId).HasColumnName("MaintenanceID");
            entity.Property(e => e.MaintencaceName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MaintenanceRequest>(entity =>
        {
            entity.HasKey(e => new { e.MaintenanceRequestId, e.RequestId }).HasName("PK__Maintena__F3E230DB679F2A25");

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

            entity.HasOne(d => d.MaintenanceRequestNavigation).WithMany(p => p.MaintenanceRequests)
                .HasForeignKey(d => d.MaintenanceRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Maintenan__Maint__36470DEF");

            entity.HasOne(d => d.Request).WithMany(p => p.MaintenanceRequests)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Maintenan__Reque__373B3228");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Request__33A8519A84AC8086");

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

            entity.HasOne(d => d.Design).WithMany(p => p.Requests)
                .HasForeignKey(d => d.DesignId)
                .HasConstraintName("FK_Request_Design");

            entity.HasOne(d => d.Sample).WithMany(p => p.Requests)
                .HasForeignKey(d => d.SampleId)
                .HasConstraintName("FK_Request_Sample");

            entity.HasOne(d => d.User).WithMany(p => p.Requests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Request__UserID__2BC97F7C");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A53C836D1");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Sample>(entity =>
        {
            entity.HasKey(e => e.SampleId).HasName("PK__Sample__8B99EC0AC9E00B24");

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

            entity.HasOne(d => d.ConstructionType).WithMany(p => p.Samples)
                .HasForeignKey(d => d.ConstructionTypeId)
                .HasConstraintName("FK__Sample__Construc__2610A626");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC1C7C11F0");

            entity.HasIndex(e => e.AccountId, "UQ__Users__349DA587AEC5FC45").IsUnique();

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

            entity.HasOne(d => d.Account).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.AccountId)
                .HasConstraintName("FK__Users__AccountID__2057CCD0");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__RoleID__214BF109");
        });
        modelBuilder.HasSequence<int>("SalesOrderNumber", "SalesLT");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
