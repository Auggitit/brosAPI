using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuggitAPIServer.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "qtymt",
                table: "vSODetails");

            migrationBuilder.RenameColumn(
                name: "qtymt",
                table: "vSSODetails",
                newName: "uomcode");

            migrationBuilder.RenameColumn(
                name: "qtymt",
                table: "vSSalesDetails",
                newName: "uomcode");

            migrationBuilder.RenameColumn(
                name: "qtymt",
                table: "vSalesDetails",
                newName: "uomcode");

            migrationBuilder.AddColumn<decimal>(
                name: "uom",
                table: "vSSODetails",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "uom",
                table: "vSSalesDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "uom",
                table: "vSODetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "uomcode",
                table: "vSODetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "uom",
                table: "vSalesDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "uomcoode",
                table: "vDRDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "uomcode",
                table: "vCRDetails",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "uom",
                table: "vSSODetails");

            migrationBuilder.DropColumn(
                name: "uom",
                table: "vSSalesDetails");

            migrationBuilder.DropColumn(
                name: "uom",
                table: "vSODetails");

            migrationBuilder.DropColumn(
                name: "uomcode",
                table: "vSODetails");

            migrationBuilder.DropColumn(
                name: "uom",
                table: "vSalesDetails");

            migrationBuilder.DropColumn(
                name: "uomcoode",
                table: "vDRDetails");

            migrationBuilder.DropColumn(
                name: "uomcode",
                table: "vCRDetails");

            migrationBuilder.RenameColumn(
                name: "uomcode",
                table: "vSSODetails",
                newName: "qtymt");

            migrationBuilder.RenameColumn(
                name: "uomcode",
                table: "vSSalesDetails",
                newName: "qtymt");

            migrationBuilder.RenameColumn(
                name: "uomcode",
                table: "vSalesDetails",
                newName: "qtymt");

            migrationBuilder.AddColumn<decimal>(
                name: "qtymt",
                table: "vSODetails",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
