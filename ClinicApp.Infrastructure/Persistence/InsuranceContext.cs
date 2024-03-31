using ClinicApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ClinicApp.Infrastructure.Persistence;
public class InsuranceContext : DbContext
{
    public InsuranceContext()
    {
    }

    public InsuranceContext(DbContextOptions<InsuranceContext> options)
        : base(options)
    {
    }

    #region Fields
    public DbSet<Agreement> Agreements { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Contractor> Contractors { get; set; }
    public DbSet<ContractorType> ContractorTypes { get; set; }
    public DbSet<Diagnosis> Diagnoses { get; set; }
    public DbSet<PatientAccount> PatientAccounts { get; set; }
    public DbSet<Payroll> Payrolls { get; set; }
    public DbSet<Period> Periods { get; set; }
    public DbSet<PlaceOfService> PlacesOfService { get; set; }
    public DbSet<Procedure> Procedures { get; set; }
    public DbSet<ReleaseInformation> ReleaseInformations { get; set; }
    public DbSet<ServiceLog> ServiceLogs { get; set; }
    public DbSet<UnitDetail> UnitDetails { get; set; }
    public DbSet<Insurance> Insurances { get; set; }
    public DbSet<InsuranceProcedure> InsuranceProcedures { get; set; }
    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=lin-13704-4133-pgsql-primary.servers.linodedb.net;Database=insurancedb;Username=linpostgres;Password=HxywGpAs2-2CnbGh");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseUtcDateTimeConverter();

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Agreement>()
            .HasOne(a => a.Client)
            .WithMany(c => c.Agreements)
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Client>()
            .HasMany(c => c.Agreements)
            .WithOne(a => a.Client)
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Contractor>()
            .HasMany(c => c.Payrolls)
            .WithOne(p => p.Contractor)
            .HasForeignKey(p => p.ContractorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Insurance>()
            .HasMany(i => i.InsuranceProcedures)
            .WithOne(ip => ip.Insurance)
            .HasForeignKey(ip => ip.InsuranceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ServiceLog>()
            .HasMany(sl => sl.UnitDetails)
            .WithOne(ud => ud.ServiceLog)
            .HasForeignKey(ud => ud.ServiceLogId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InsuranceProcedure>()
            .HasOne(ip => ip.Insurance)
            .WithMany(i => i.InsuranceProcedures)
            .HasForeignKey(ip => ip.InsuranceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InsuranceProcedure>()
            .HasOne(ip => ip.Procedure)
            .WithMany()
            .HasForeignKey(ip => ip.ProcedureId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Payroll>()
            .HasOne(p => p.InsuranceProcedure)
            .WithMany()
            .HasForeignKey(p => p.InsuranceProcedureId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UnitDetail>()
            .HasOne(ud => ud.Procedure)
            .WithMany()
            .HasForeignKey(ud => ud.ProcedureId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UnitDetail>()
            .HasOne(ud => ud.PlaceOfService)
            .WithMany()
            .HasForeignKey(ud => ud.PlaceOfServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PatientAccount>().ToTable("PatientAccount");
        modelBuilder.Entity<Agreement>().ToTable("Agreement");
        modelBuilder.Entity<Client>().ToTable("Client");
        modelBuilder.Entity<Company>().ToTable("Company");
        modelBuilder.Entity<Contractor>().ToTable("Contractor");
        modelBuilder.Entity<ContractorType>().ToTable("ContractorType");
        modelBuilder.Entity<Diagnosis>().ToTable("Diagnosis");
        modelBuilder.Entity<Payroll>().ToTable("Payroll");
        modelBuilder.Entity<Period>().ToTable("Period");
        modelBuilder.Entity<PlaceOfService>().ToTable("PlaceOfService");
        modelBuilder.Entity<Procedure>().ToTable("Procedure");
        modelBuilder.Entity<ReleaseInformation>().ToTable("ReleaseInformation");
        modelBuilder.Entity<ServiceLog>().ToTable("ServiceLog");
        modelBuilder.Entity<UnitDetail>().ToTable("UnitDetail");
        modelBuilder.Entity<Insurance>().ToTable("Insurance");
        modelBuilder.Entity<InsuranceProcedure>().ToTable("InsuranceProcedure");
    }

}

public class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public UtcDateTimeConverter() : base(
        v => v.ToUniversalTime(),
        v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
    {
    }
}

public static class ModelBuilderExtensions
{
    public static void UseUtcDateTimeConverter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(new UtcDateTimeConverter());
                }
            }
        }
    }
}
