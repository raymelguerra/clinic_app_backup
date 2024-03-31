using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicApp.Core.Migrations
{
    /// <inheritdoc />
    public partial class insurancedbfixrecipientIdtoclient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecipientID",
                table: "Client",
                newName: "RecipientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecipientId",
                table: "Client",
                newName: "RecipientID");
        }
    }
}
