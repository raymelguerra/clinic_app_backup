using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClinicApp.Core.Migrations
{
    /// <inheritdoc />
    public partial class insurancedbfirstmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Acronym = table.Column<string>(type: "text", nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contractor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RenderingProvider = table.Column<string>(type: "text", nullable: true),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    Extra = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractorType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractorType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Diagnosis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnosis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Insurance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Logo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insurance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Period",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    PayPeriod = table.Column<string>(type: "text", nullable: false),
                    DocumentDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Period", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaceOfService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceOfService", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Procedure",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedure", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceProcedure",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InsuranceId = table.Column<int>(type: "integer", nullable: false),
                    ProcedureId = table.Column<int>(type: "integer", nullable: false),
                    Rate = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceProcedure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsuranceProcedure_Insurance_InsuranceId",
                        column: x => x.InsuranceId,
                        principalTable: "Insurance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsuranceProcedure_Procedure_ProcedureId",
                        column: x => x.ProcedureId,
                        principalTable: "Procedure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RecipientID = table.Column<string>(type: "text", nullable: false),
                    PatientAccount = table.Column<string>(type: "text", nullable: false),
                    ReleaseInformationId = table.Column<int>(type: "integer", nullable: false),
                    ReferringProvider = table.Column<string>(type: "text", nullable: true),
                    AuthorizationNumber = table.Column<string>(type: "text", nullable: true),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    DiagnosisId = table.Column<int>(type: "integer", nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    WeeklyApprovedRBT = table.Column<int>(type: "integer", nullable: false),
                    WeeklyApprovedAnalyst = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_Diagnosis_DiagnosisId",
                        column: x => x.DiagnosisId,
                        principalTable: "Diagnosis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Client_ReleaseInformation_ReleaseInformationId",
                        column: x => x.ReleaseInformationId,
                        principalTable: "ReleaseInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContractorId = table.Column<int>(type: "integer", nullable: false),
                    ContractorTypeId = table.Column<int>(type: "integer", nullable: false),
                    InsuranceProcedureId = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payroll_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payroll_ContractorType_ContractorTypeId",
                        column: x => x.ContractorTypeId,
                        principalTable: "ContractorType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payroll_Contractor_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payroll_InsuranceProcedure_InsuranceProcedureId",
                        column: x => x.InsuranceProcedureId,
                        principalTable: "InsuranceProcedure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LicenseNumber = table.Column<string>(type: "text", nullable: false),
                    Auxiliar = table.Column<string>(type: "text", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAccount_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PeriodId = table.Column<int>(type: "integer", nullable: false),
                    ContractorId = table.Column<int>(type: "integer", nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    InsuranceId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BilledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Biller = table.Column<string>(type: "text", nullable: false),
                    Pending = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceLog_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceLog_Contractor_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceLog_Insurance_InsuranceId",
                        column: x => x.InsuranceId,
                        principalTable: "Insurance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceLog_Period_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Period",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agreement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    PayrollId = table.Column<int>(type: "integer", nullable: false),
                    RateEmployees = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agreement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agreement_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agreement_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agreement_Payroll_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payroll",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnitDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Modifiers = table.Column<string>(type: "text", nullable: true),
                    PlaceOfServiceId = table.Column<int>(type: "integer", nullable: false),
                    DateOfService = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Unit = table.Column<int>(type: "integer", nullable: false),
                    ServiceLogId = table.Column<int>(type: "integer", nullable: false),
                    ProcedureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitDetail_PlaceOfService_PlaceOfServiceId",
                        column: x => x.PlaceOfServiceId,
                        principalTable: "PlaceOfService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitDetail_Procedure_ProcedureId",
                        column: x => x.ProcedureId,
                        principalTable: "Procedure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitDetail_ServiceLog_ServiceLogId",
                        column: x => x.ServiceLogId,
                        principalTable: "ServiceLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agreement_ClientId",
                table: "Agreement",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Agreement_CompanyId",
                table: "Agreement",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Agreement_PayrollId",
                table: "Agreement",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_DiagnosisId",
                table: "Client",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_ReleaseInformationId",
                table: "Client",
                column: "ReleaseInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceProcedure_InsuranceId",
                table: "InsuranceProcedure",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceProcedure_ProcedureId",
                table: "InsuranceProcedure",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAccount_ClientId",
                table: "PatientAccount",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_CompanyId",
                table: "Payroll",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_ContractorId",
                table: "Payroll",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_ContractorTypeId",
                table: "Payroll",
                column: "ContractorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_InsuranceProcedureId",
                table: "Payroll",
                column: "InsuranceProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLog_ClientId",
                table: "ServiceLog",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLog_ContractorId",
                table: "ServiceLog",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLog_InsuranceId",
                table: "ServiceLog",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLog_PeriodId",
                table: "ServiceLog",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitDetail_PlaceOfServiceId",
                table: "UnitDetail",
                column: "PlaceOfServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitDetail_ProcedureId",
                table: "UnitDetail",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitDetail_ServiceLogId",
                table: "UnitDetail",
                column: "ServiceLogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agreement");

            migrationBuilder.DropTable(
                name: "PatientAccount");

            migrationBuilder.DropTable(
                name: "UnitDetail");

            migrationBuilder.DropTable(
                name: "Payroll");

            migrationBuilder.DropTable(
                name: "PlaceOfService");

            migrationBuilder.DropTable(
                name: "ServiceLog");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "ContractorType");

            migrationBuilder.DropTable(
                name: "InsuranceProcedure");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Contractor");

            migrationBuilder.DropTable(
                name: "Period");

            migrationBuilder.DropTable(
                name: "Insurance");

            migrationBuilder.DropTable(
                name: "Procedure");

            migrationBuilder.DropTable(
                name: "Diagnosis");

            migrationBuilder.DropTable(
                name: "ReleaseInformation");
        }
    }
}
