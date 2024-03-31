using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicApp.Core.Migrations
{
    /// <inheritdoc />
    public partial class Addcontractortypetoprocedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContractorTypeId",
                table: "Procedure",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Procedure_ContractorTypeId",
                table: "Procedure",
                column: "ContractorTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Procedure_ContractorType_ContractorTypeId",
                table: "Procedure",
                column: "ContractorTypeId",
                principalTable: "ContractorType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Procedure_ContractorType_ContractorTypeId",
                table: "Procedure");

            migrationBuilder.DropIndex(
                name: "IX_Procedure_ContractorTypeId",
                table: "Procedure");

            migrationBuilder.DropColumn(
                name: "ContractorTypeId",
                table: "Procedure");
        }
    }
}
