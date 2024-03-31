using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicApp.Core.Migrations
{
    /// <inheritdoc />
    public partial class fixonetoonerelationbetweenpatientandpatientaccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAccount_Client_ClientId",
                table: "PatientAccount");

            migrationBuilder.DropIndex(
                name: "IX_PatientAccount_ClientId",
                table: "PatientAccount");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "PatientAccount");

            migrationBuilder.AddColumn<int>(
                name: "PatientAccountId",
                table: "Client",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Client_PatientAccountId",
                table: "Client",
                column: "PatientAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_PatientAccount_PatientAccountId",
                table: "Client",
                column: "PatientAccountId",
                principalTable: "PatientAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_PatientAccount_PatientAccountId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_PatientAccountId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "PatientAccountId",
                table: "Client");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "PatientAccount",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PatientAccount_ClientId",
                table: "PatientAccount",
                column: "ClientId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAccount_Client_ClientId",
                table: "PatientAccount",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
