using Microsoft.EntityFrameworkCore;
using ClinicApp.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace ClinicApp.Core.Data;

public partial class clinicbdContext : IdentityDbContext
{
    
    public clinicbdContext()
    {
    }

    public clinicbdContext(DbContextOptions<clinicbdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agreement> Agreements { get; set; } = null!;
    public virtual DbSet<Billing> Billings { get; set; } = null!;
    public virtual DbSet<Client> Clients { get; set; } = null!;
    public virtual DbSet<Company> Companies { get; set; } = null!;
    public virtual DbSet<Contractor> Contractors { get; set; } = null!;
    public virtual DbSet<ContractorType> ContractorTypes { get; set; } = null!;
    public virtual DbSet<Diagnosis> Diagnoses { get; set; } = null!;
    public virtual DbSet<PatientAccount> PatientAccounts { get; set; } = null!;
    public virtual DbSet<Payroll> Payrolls { get; set; } = null!;
    public virtual DbSet<Period> Periods { get; set; } = null!;
    public virtual DbSet<PlaceOfService> PlaceOfServices { get; set; } = null!;
    public virtual DbSet<Procedure> Procedures { get; set; } = null!;
    public virtual DbSet<ReleaseInformation> ReleaseInformations { get; set; } = null!;
    public virtual DbSet<ServiceLog> ServiceLogs { get; set; } = null!;
    public virtual DbSet<SubProcedure> SubProcedures { get; set; } = null!;
    public virtual DbSet<UnitDetail> UnitDetails { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", false)
               .Build();
            var bd = config.GetConnectionString("clinicbdContext") ?? throw new ArgumentNullException(nameof(config));
            optionsBuilder.UseSqlServer(bd);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agreement>(entity =>
        {
            entity.ToTable("Agreement");

            entity.HasIndex(e => e.ClientId, "IX_FK_ClientAgreement");

            entity.HasIndex(e => e.CompanyId, "IX_FK_CompanyAgreement");

            entity.HasIndex(e => e.PayrollId, "IX_FK_PayrollAgreement");

            entity.HasOne(d => d.Client)
                .WithMany(p => p.Agreements)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientAgreement");

            entity.HasOne(d => d.Company)
                .WithMany(p => p.Agreements)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompanyAgreement");

            entity.HasOne(d => d.Payroll)
                .WithMany(p => p.Agreements)
                .HasForeignKey(d => d.PayrollId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PayrollAgreement");
        });

        modelBuilder.Entity<Billing>(entity =>
        {
            entity.ToTable("Billing");

            entity.HasIndex(e => e.ClientId, "IX_FK_ClientBilling");

            entity.HasIndex(e => e.ContractorId, "IX_FK_ContractorBilling");

            entity.HasIndex(e => e.PeriodId, "IX_FK_PeriodBilling");

            entity.Property(e => e.BillingDate).HasColumnType("datetime");

            entity.HasOne(d => d.Client)
                .WithMany(p => p.Billings)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientBilling");

            entity.HasOne(d => d.Contractor)
                .WithMany(p => p.Billings)
                .HasForeignKey(d => d.ContractorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractorBilling");

            entity.HasOne(d => d.Period)
                .WithMany(p => p.Billings)
                .HasForeignKey(d => d.PeriodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PeriodBilling");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Client");

            entity.HasIndex(e => e.DiagnosisId, "IX_FK_DiagnosisClient");

            entity.HasIndex(e => e.ReleaseInformationId, "IX_FK_ReleaseInformationClient");

            entity.Property(e => e.AuthorizationNumber).HasColumnName("AuthorizationNUmber");

            entity.Property(e => e.RecipientId).HasColumnName("RecipientID");

            entity.Property(e => e.WeeklyApprovedRbt).HasColumnName("WeeklyApprovedRBT");

            entity.HasOne(d => d.Diagnosis)
                .WithMany(p => p.Clients)
                .HasForeignKey(d => d.DiagnosisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DiagnosisClient");

            entity.HasOne(d => d.ReleaseInformation)
                .WithMany(p => p.Clients)
                .HasForeignKey(d => d.ReleaseInformationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReleaseInformationClient");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");
        });

        modelBuilder.Entity<Contractor>(entity =>
        {
            entity.ToTable("Contractor");
        });

        modelBuilder.Entity<ContractorType>(entity =>
        {
            entity.ToTable("ContractorType");
        });

        modelBuilder.Entity<Diagnosis>(entity =>
        {
            entity.ToTable("Diagnosis");
        });

        modelBuilder.Entity<PatientAccount>(entity =>
        {
            entity.ToTable("PatientAccount");

            entity.HasIndex(e => e.ClientId, "IX_FK_ClientPatientAccount");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Client)
                .WithMany(p => p.PatientAccounts)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientPatientAccount");
        });

        modelBuilder.Entity<Payroll>(entity =>
        {
            entity.ToTable("Payroll");

            entity.HasIndex(e => e.CompanyId, "IX_FK_CompanyPayroll");

            entity.HasIndex(e => e.ContractorId, "IX_FK_ContractorPayroll");

            entity.HasIndex(e => e.ContractorTypeId, "IX_FK_ContractorTypePayroll");

            entity.HasIndex(e => e.ProcedureId, "IX_FK_ProcedurePayroll");

            entity.HasOne(d => d.Company)
                .WithMany(p => p.Payrolls)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompanyPayroll");

            entity.HasOne(d => d.Contractor)
                .WithMany(p => p.Payrolls)
                .HasForeignKey(d => d.ContractorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractorPayroll");

            entity.HasOne(d => d.ContractorType)
                .WithMany(p => p.Payrolls)
                .HasForeignKey(d => d.ContractorTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractorTypePayroll");

            entity.HasOne(d => d.Procedure)
                .WithMany(p => p.Payrolls)
                .HasForeignKey(d => d.ProcedureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcedurePayroll");
        });

        modelBuilder.Entity<Period>(entity =>
        {
            entity.ToTable("Period");

            entity.Property(e => e.DocumentDeliveryDate).HasColumnType("datetime");

            entity.Property(e => e.EndDate).HasColumnType("datetime");

            entity.Property(e => e.PaymentDate).HasColumnType("datetime");

            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<PlaceOfService>(entity =>
        {
            entity.ToTable("PlaceOfService");
        });

        modelBuilder.Entity<Procedure>(entity =>
        {
            entity.ToTable("Procedure");
        });

        modelBuilder.Entity<ReleaseInformation>(entity =>
        {
            entity.ToTable("ReleaseInformation");
        });

        modelBuilder.Entity<ServiceLog>(entity =>
        {
            entity.ToTable("ServiceLog");

            entity.HasIndex(e => e.ClientId, "IX_FK_ClientServiceLog");

            entity.HasIndex(e => e.ContractorId, "IX_FK_ContractorServiceLog");

            entity.HasIndex(e => e.PeriodId, "IX_FK_PeriodServiceLog");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Client)
                .WithMany(p => p.ServiceLogs)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientServiceLog");

            entity.HasOne(d => d.Contractor)
                .WithMany(p => p.ServiceLogs)
                .HasForeignKey(d => d.ContractorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractorServiceLog");

            entity.HasOne(d => d.Period)
                .WithMany(p => p.ServiceLogs)
                .HasForeignKey(d => d.PeriodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PeriodServiceLog");
        });

        modelBuilder.Entity<SubProcedure>(entity =>
        {
            entity.ToTable("SubProcedure");

            entity.HasIndex(e => e.ProcedureId, "IX_FK_ProcedureSubProcedure");

            entity.HasOne(d => d.Procedure)
                .WithMany(p => p.SubProcedures)
                .HasForeignKey(d => d.ProcedureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcedureSubProcedure");
        });

        modelBuilder.Entity<UnitDetail>(entity =>
        {
            entity.ToTable("UnitDetail");

            entity.HasIndex(e => e.PlaceOfServiceId, "IX_FK_PlaceOfServiceUnitDetail");

            entity.HasIndex(e => e.ServiceLogId, "IX_FK_ServiceLogUnitDetail");

            entity.HasIndex(e => e.SubProcedureId, "IX_FK_SubProcedureUnitDetail");

            entity.Property(e => e.DateOfService).HasColumnType("datetime");

            entity.HasOne(d => d.PlaceOfService)
                .WithMany(p => p.UnitDetails)
                .HasForeignKey(d => d.PlaceOfServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlaceOfServiceUnitDetail");

            entity.HasOne(d => d.ServiceLog)
                .WithMany(p => p.UnitDetails)
                .HasForeignKey(d => d.ServiceLogId)
                .HasConstraintName("FK_ServiceLogUnitDetail");

            entity.HasOne(d => d.SubProcedure)
                .WithMany(p => p.UnitDetails)
                .HasForeignKey(d => d.SubProcedureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubProcedureUnitDetail");
        });

        base.OnModelCreating(modelBuilder);

        // Initializers database
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        
        // OnModelCreatingPartial(modelBuilder);
    }

    // partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
