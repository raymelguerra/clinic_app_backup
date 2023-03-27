using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ClinicApp.Core.Models;
using ClinicApp.Core.DTO;

namespace ClinicApp.Core.Data;

public partial class ClinicbdMigrationContext : IdentityDbContext
{
    public ClinicbdMigrationContext(DbContextOptions<ClinicbdMigrationContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        Database.SetCommandTimeout(60);
    }

    public virtual DbSet<ProfitHistory>? ProfitHistory { get; set; }
    public virtual DbSet<ServicesLogStatus>? ServicesLogStatus { get; set; }
    public virtual DbSet<ServiceLogWithoutPatientAccount>? ServiceLogWithoutPatientAccount { get; set; }
    public virtual DbSet<Agreement> Agreements { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Contractor> Contractors { get; set; }

    public virtual DbSet<ContractorType> ContractorTypes { get; set; }

    public virtual DbSet<Diagnosis> Diagnoses { get; set; }

    public virtual DbSet<PatientAccount> PatientAccounts { get; set; }

    public virtual DbSet<Payroll> Payrolls { get; set; }

    public virtual DbSet<Period> Periods { get; set; }

    public virtual DbSet<PlaceOfService> PlaceOfServices { get; set; }

    public virtual DbSet<Procedure> Procedures { get; set; }

    public virtual DbSet<ReleaseInformation> ReleaseInformations { get; set; }

    public virtual DbSet<ServiceLog> ServiceLogs { get; set; }

    public virtual DbSet<SubProcedure> SubProcedures { get; set; }

    public virtual DbSet<UnitDetail> UnitDetails { get; set; }
    public virtual DbSet<ContractorServiceLog> ContractorServiceLog { get; set; }
    public virtual DbSet<PatientUnitDetail> PatientUnitDetail { get; set; }
    public virtual DbSet<ContractorUser> ContractorUser { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseNpgsql("Host=lin-13704-4133-pgsql-primary.servers.linodedb.net;Database=aba_test;Username=linpostgres;Password=HxywGpAs2-2CnbGh");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Agreement>(entity =>
        {
            entity.ToTable("Agreement");

            entity.HasIndex(e => e.ClientId, "IX_FK_ClientAgreement");

            entity.HasIndex(e => e.CompanyId, "IX_FK_CompanyAgreement");

            entity.HasIndex(e => e.PayrollId, "IX_FK_PayrollAgreement");

            entity.HasOne(d => d.Client).WithMany(p => p.Agreements)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientAgreement");

            entity.HasOne(d => d.Company).WithMany(p => p.Agreements)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompanyAgreement");

            entity.HasOne(d => d.Payroll).WithMany(p => p.Agreements)
                .HasForeignKey(d => d.PayrollId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PayrollAgreement");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Client");

            entity.HasIndex(e => e.DiagnosisId, "IX_FK_DiagnosisClient");

            entity.HasIndex(e => e.ReleaseInformationId, "IX_FK_ReleaseInformationClient");

            entity.Property(e => e.AuthorizationNumber)
                .HasMaxLength(20)
                .HasColumnName("AuthorizationNUmber");
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.PatientAccount).HasMaxLength(20);
            entity.Property(e => e.RecipientId)
                .HasMaxLength(20)
                .HasColumnName("RecipientID");
            entity.Property(e => e.ReferringProvider).HasMaxLength(20);
            entity.Property(e => e.WeeklyApprovedRbt).HasColumnName("WeeklyApprovedRBT");

            entity.HasOne(d => d.Diagnosis).WithMany(p => p.Clients)
                .HasForeignKey(d => d.DiagnosisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DiagnosisClient");

            entity.HasOne(d => d.ReleaseInformation).WithMany(p => p.Clients)
                .HasForeignKey(d => d.ReleaseInformationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReleaseInformationClient");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");

            entity.Property(e => e.Acronym).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Contractor>(entity =>
        {
            entity.ToTable("Contractor");

            entity.Property(e => e.Extra).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.RenderingProvider).HasMaxLength(20);

            entity.HasOne(d => d.ContractorUser).WithOne(p => p.Contractor)
                .HasForeignKey<ContractorUser>(d => d.ContractorId);
        });

        modelBuilder.Entity<ContractorType>(entity =>
        {
            entity.ToTable("ContractorType");

            entity.Property(e => e.Name).HasMaxLength(128);
        });

        modelBuilder.Entity<Diagnosis>(entity =>
        {
            entity.ToTable("Diagnosis");

            entity.Property(e => e.Description).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(128);
        });

        modelBuilder.Entity<PatientAccount>(entity =>
        {
            entity.ToTable("PatientAccount");

            entity.HasIndex(e => e.ClientId, "IX_FK_ClientPatientAccount");

            entity.Property(e => e.Auxiliar).HasMaxLength(20);
            entity.Property(e => e.CreateDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.ExpireDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.LicenseNumber).HasMaxLength(20);

            entity.HasOne(d => d.Client).WithMany(p => p.PatientAccounts)
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

            entity.HasOne(d => d.Company).WithMany(p => p.Payrolls)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompanyPayroll");

            entity.HasOne(d => d.Contractor).WithMany(p => p.Payrolls)
                .HasForeignKey(d => d.ContractorId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("FK_ContractorPayroll");

            entity.HasOne(d => d.ContractorType).WithMany(p => p.Payrolls)
                .HasForeignKey(d => d.ContractorTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractorTypePayroll");

            entity.HasOne(d => d.Procedure).WithMany(p => p.Payrolls)
                .HasForeignKey(d => d.ProcedureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcedurePayroll");
        });

        modelBuilder.Entity<Period>(entity =>
        {
            entity.ToTable("Period");

            entity.Property(e => e.DocumentDeliveryDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.EndDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.PayPeriod).HasMaxLength(20);
            entity.Property(e => e.PaymentDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.StartDate).HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<PlaceOfService>(entity =>
        {
            entity.ToTable("PlaceOfService");

            entity.Property(e => e.Name).HasMaxLength(128);
            entity.Property(e => e.Value).HasMaxLength(5);
        });

        modelBuilder.Entity<Procedure>(entity =>
        {
            entity.ToTable("Procedure");

            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<ReleaseInformation>(entity =>
        {
            entity.ToTable("ReleaseInformation");

            entity.Property(e => e.Name).HasMaxLength(128);
        });

        modelBuilder.Entity<ServiceLog>(entity =>
        {
            entity.ToTable("ServiceLog");

            entity.HasIndex(e => e.ClientId, "IX_FK_ClientServiceLog");

            entity.HasIndex(e => e.ContractorId, "IX_FK_ContractorServiceLog");

            entity.HasIndex(e => e.PeriodId, "IX_FK_PeriodServiceLog");

            entity.Property(e => e.BilledDate).HasColumnType("timestamp with time zone");
            entity.Property(e => e.Biller).HasMaxLength(450);
            entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");
            entity.Property(e => e.Pending).HasMaxLength(500);
            entity.Property(e => e.Status);//.HasDefaultValue(0);

            entity.HasOne(d => d.Client).WithMany(p => p.ServiceLogs)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientServiceLog");

            entity.HasOne(d => d.Contractor).WithMany(p => p.ServiceLogs)
                .HasForeignKey(d => d.ContractorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractorServiceLog");

            entity.HasOne(d => d.Period).WithMany(p => p.ServiceLogs)
                .HasForeignKey(d => d.PeriodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PeriodServiceLog");

            entity.HasOne(d => d.ContractorServiceLog).WithOne(p => p.ServiceLog)
               .HasForeignKey<ContractorServiceLog>(d => d.ServiceLogId);
        });

        modelBuilder.Entity<SubProcedure>(entity =>
        {
            entity.ToTable("SubProcedure");

            entity.HasIndex(e => e.ProcedureId, "IX_FK_ProcedureSubProcedure");

            entity.Property(e => e.Name).HasMaxLength(20);

            entity.HasOne(d => d.Procedure).WithMany(p => p.SubProcedures)
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

            entity.Property(e => e.DateOfService).HasColumnType("timestamp with time zone");
            entity.Property(e => e.Modifiers).HasMaxLength(5);

            entity.HasOne(d => d.PlaceOfService).WithMany(p => p.UnitDetails)
                .HasForeignKey(d => d.PlaceOfServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlaceOfServiceUnitDetail");

            entity.HasOne(d => d.ServiceLog).WithMany(p => p.UnitDetails)
                .HasForeignKey(d => d.ServiceLogId)
                .HasConstraintName("FK_ServiceLogUnitDetail");

            entity.HasOne(d => d.SubProcedure).WithMany(p => p.UnitDetails)
                .HasForeignKey(d => d.SubProcedureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubProcedureUnitDetail");

            entity.HasOne(d => d.PatientUnitDetail).WithOne(p => p.UnitDetail)
               .HasForeignKey<PatientUnitDetail>(d => d.UnitDetailId);
        });
        
        modelBuilder.Entity<ContractorServiceLog>(entity =>
        {
            entity.ToTable("ContractorServiceLog");

            entity.HasIndex(e => e.ServiceLogId, "IX_FK_ServiceLogContractorServiceLog");

            entity.Property(e => e.Signature).HasMaxLength(int.MaxValue);
            entity.Property(e => e.SignatureDate).HasColumnType("timestamp without time zone");


        });

        modelBuilder.Entity<PatientUnitDetail>(entity =>
        {
            entity.ToTable("PatientUnitDetail");

            entity.HasIndex(e => e.UnitDetailId, "IX_FK_UnitDetailPatientUnitDetail");

            entity.Property(e => e.Signature).HasMaxLength(int.MaxValue);
            entity.Property(e => e.SignatureDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.EntryTime).HasMaxLength(20);
            entity.Property(e => e.DepartureTime).HasMaxLength(20);



        });
        
        modelBuilder.Entity<ContractorUser>(entity =>
        {
            entity.ToTable("ContractorUser");

            entity.HasIndex(e => e.UserId, "IX_FK_UserContractorUser");

            entity.HasIndex(e => e.ContractorId, "IX_FK_ContractorContracotUser");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
