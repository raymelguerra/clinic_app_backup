﻿// <auto-generated />
using System;
using ClinicApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClinicApp.Core.Migrations
{
    [DbContext(typeof(InsuranceContext))]
    [Migration("20240330072417_remove time zone to datetime in patientaccount")]
    partial class removetimezonetodatetimeinpatientaccount
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ClinicApp.Core.Models.Agreement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ClientId")
                        .HasColumnType("integer");

                    b.Property<int>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<int>("PayrollId")
                        .HasColumnType("integer");

                    b.Property<double>("RateEmployees")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("PayrollId");

                    b.ToTable("Agreement", (string)null);
                });

            modelBuilder.Entity("ClinicApp.Core.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AuthorizationNumber")
                        .HasColumnType("text");

                    b.Property<int>("DiagnosisId")
                        .HasColumnType("integer");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PatientAccount")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PatientAccountId")
                        .HasColumnType("integer");

                    b.Property<string>("RecipientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ReferringProvider")
                        .HasColumnType("text");

                    b.Property<int>("ReleaseInformationId")
                        .HasColumnType("integer");

                    b.Property<int>("Sequence")
                        .HasColumnType("integer");

                    b.Property<int>("WeeklyApprovedAnalyst")
                        .HasColumnType("integer");

                    b.Property<int>("WeeklyApprovedRBT")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DiagnosisId");

                    b.HasIndex("PatientAccountId");

                    b.HasIndex("ReleaseInformationId");

                    b.ToTable("Client", (string)null);
                });

            modelBuilder.Entity("ClinicApp.Core.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Acronym")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Company", (string)null);
                });

            modelBuilder.Entity("ClinicApp.Core.Models.Contractor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean");

                    b.Property<string>("Extra")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RenderingProvider")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Contractor", (string)null);
                });

            modelBuilder.Entity("ClinicApp.Core.Models.ContractorType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ContractorType", (string)null);
                });

            modelBuilder.Entity("ClinicApp.Core.Models.Diagnosis", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Diagnosis", (string)null);
                });

            modelBuilder.Entity("ClinicApp.Core.Models.Insurance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Insurance", (string)null);
                });

            modelBuilder.Entity("ClinicApp.Core.Models.InsuranceProcedure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("InsuranceId")
                        .HasColumnType("integer");

                    b.Property<int>("ProcedureId")
                        .HasColumnType("integer");

                    b.Property<double>("Rate")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("InsuranceId");

                    b.HasIndex("ProcedureId");

                    b.ToTable("InsuranceProcedure", (string)null);
                });

            modelBuilder.Entity("ClinicApp.Core.Models.PatientAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Auxiliar")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ExpireDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LicenseNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PatientAccount", (string)null);
                });

            modelBuilder.Entity("ClinicApp.Core.Models.Payroll", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<int>("ContractorId")
                        .HasColumnType("integer");

                    b.Property<int>("ContractorTypeId")
                        .HasColumnType("integer");

                    b.Property<int>("InsuranceProcedureId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ContractorId");

                    b.HasIndex("ContractorTypeId");

                    b.HasIndex("InsuranceProcedureId");

                    b.ToTable("Payroll", (string)null);
                });

            modelBuilder.Entity("ClinicApp.Core.Models.Period", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("DocumentDeliveryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PayPeriod")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Period", (string)null);
                });

            modelBuilder.Entity("ClinicApp.Core.Models.PlaceOfService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PlaceOfService", (string)null);
                });

            modelBuilder.Entity("ClinicApp.Core.Models.Procedure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Procedure", (string)null);
                });

            modelBuilder.Entity("ClinicApp.Core.Models.ReleaseInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ReleaseInformation", (string)null);
                });

            modelBuilder.Entity("ClinicApp.Core.Models.ServiceLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("BilledDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Biller")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ClientId")
                        .HasColumnType("integer");

                    b.Property<int>("ContractorId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("InsuranceId")
                        .HasColumnType("integer");

                    b.Property<string>("Pending")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PeriodId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("ContractorId");

                    b.HasIndex("InsuranceId");

                    b.HasIndex("PeriodId");

                    b.ToTable("ServiceLog", (string)null);
                });

            modelBuilder.Entity("ClinicApp.Core.Models.UnitDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfService")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Modifiers")
                        .HasColumnType("text");

                    b.Property<int>("PlaceOfServiceId")
                        .HasColumnType("integer");

                    b.Property<int>("ProcedureId")
                        .HasColumnType("integer");

                    b.Property<int>("ServiceLogId")
                        .HasColumnType("integer");

                    b.Property<int>("Unit")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PlaceOfServiceId");

                    b.HasIndex("ProcedureId");

                    b.HasIndex("ServiceLogId");

                    b.ToTable("UnitDetail", (string)null);
                });

            modelBuilder.Entity("ClinicApp.Core.Models.Agreement", b =>
                {
                    b.HasOne("ClinicApp.Core.Models.Client", "Client")
                        .WithMany("Agreements")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicApp.Core.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicApp.Core.Models.Payroll", "Payroll")
                        .WithMany()
                        .HasForeignKey("PayrollId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Company");

                    b.Navigation("Payroll");
                });

            modelBuilder.Entity("ClinicApp.Core.Models.Client", b =>
                {
                    b.HasOne("ClinicApp.Core.Models.Diagnosis", "Diagnosis")
                        .WithMany()
                        .HasForeignKey("DiagnosisId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicApp.Core.Models.PatientAccount", "PatientAccounts")
                        .WithMany()
                        .HasForeignKey("PatientAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicApp.Core.Models.ReleaseInformation", "ReleaseInformation")
                        .WithMany()
                        .HasForeignKey("ReleaseInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Diagnosis");

                    b.Navigation("PatientAccounts");

                    b.Navigation("ReleaseInformation");
                });

            modelBuilder.Entity("ClinicApp.Core.Models.InsuranceProcedure", b =>
                {
                    b.HasOne("ClinicApp.Core.Models.Insurance", "Insurance")
                        .WithMany("InsuranceProcedures")
                        .HasForeignKey("InsuranceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicApp.Core.Models.Procedure", "Procedure")
                        .WithMany()
                        .HasForeignKey("ProcedureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Insurance");

                    b.Navigation("Procedure");
                });

            modelBuilder.Entity("ClinicApp.Core.Models.Payroll", b =>
                {
                    b.HasOne("ClinicApp.Core.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicApp.Core.Models.Contractor", "Contractor")
                        .WithMany("Payrolls")
                        .HasForeignKey("ContractorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicApp.Core.Models.ContractorType", "ContractorType")
                        .WithMany()
                        .HasForeignKey("ContractorTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicApp.Core.Models.InsuranceProcedure", "InsuranceProcedure")
                        .WithMany()
                        .HasForeignKey("InsuranceProcedureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Contractor");

                    b.Navigation("ContractorType");

                    b.Navigation("InsuranceProcedure");
                });

            modelBuilder.Entity("ClinicApp.Core.Models.ServiceLog", b =>
                {
                    b.HasOne("ClinicApp.Core.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicApp.Core.Models.Contractor", "Contractor")
                        .WithMany()
                        .HasForeignKey("ContractorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicApp.Core.Models.Insurance", "Insurance")
                        .WithMany()
                        .HasForeignKey("InsuranceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicApp.Core.Models.Period", "Period")
                        .WithMany()
                        .HasForeignKey("PeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Contractor");

                    b.Navigation("Insurance");

                    b.Navigation("Period");
                });

            modelBuilder.Entity("ClinicApp.Core.Models.UnitDetail", b =>
                {
                    b.HasOne("ClinicApp.Core.Models.PlaceOfService", "PlaceOfService")
                        .WithMany()
                        .HasForeignKey("PlaceOfServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicApp.Core.Models.Procedure", "Procedure")
                        .WithMany()
                        .HasForeignKey("ProcedureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicApp.Core.Models.ServiceLog", "ServiceLog")
                        .WithMany("UnitDetails")
                        .HasForeignKey("ServiceLogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlaceOfService");

                    b.Navigation("Procedure");

                    b.Navigation("ServiceLog");
                });

            modelBuilder.Entity("ClinicApp.Core.Models.Client", b =>
                {
                    b.Navigation("Agreements");
                });

            modelBuilder.Entity("ClinicApp.Core.Models.Contractor", b =>
                {
                    b.Navigation("Payrolls");
                });

            modelBuilder.Entity("ClinicApp.Core.Models.Insurance", b =>
                {
                    b.Navigation("InsuranceProcedures");
                });

            modelBuilder.Entity("ClinicApp.Core.Models.ServiceLog", b =>
                {
                    b.Navigation("UnitDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
