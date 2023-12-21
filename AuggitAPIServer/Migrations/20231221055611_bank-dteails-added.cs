using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuggitAPIServer.Migrations
{
    public partial class bankdteailsadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ifscCode",
                table: "mLedgers",
                newName: "ifsccode");

            migrationBuilder.RenameColumn(
                name: "accNo",
                table: "mLedgers",
                newName: "accno");

            migrationBuilder.AddColumn<string>(
                name: "accHolderName",
                table: "mLedgers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "accNo",
                table: "mLedgers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "branchName",
                table: "mLedgers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ifscCode",
                table: "mLedgers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "accHolderName",
                table: "mLedgers");

            migrationBuilder.DropColumn(
                name: "accNo",
                table: "mLedgers");

            migrationBuilder.DropColumn(
                name: "branchName",
                table: "mLedgers");

            migrationBuilder.DropColumn(
                name: "ifscCode",
                table: "mLedgers");

            migrationBuilder.RenameColumn(
                name: "ifsccode",
                table: "mLedgers",
                newName: "ifscCode");

            migrationBuilder.RenameColumn(
                name: "accno",
                table: "mLedgers",
                newName: "accNo");
        }
    }
}
