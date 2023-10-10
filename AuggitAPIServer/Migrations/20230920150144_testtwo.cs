using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuggitAPIServer.Migrations
{
    public partial class testtwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "uomcoode",
                table: "vDRDetails",
                newName: "uomcode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "uomcode",
                table: "vDRDetails",
                newName: "uomcoode");
        }
    }
}
