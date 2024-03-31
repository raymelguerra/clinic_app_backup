using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicApp.Core.Migrations
{
    /// <inheritdoc />
    public partial class removeonetomanyrelationbetweenpatientandpatientaccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PatientAccount_ClientId",
                table: "PatientAccount");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAccount_ClientId",
                table: "PatientAccount",
                column: "ClientId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PatientAccount_ClientId",
                table: "PatientAccount");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAccount_ClientId",
                table: "PatientAccount",
                column: "ClientId");
        }
    }
}
