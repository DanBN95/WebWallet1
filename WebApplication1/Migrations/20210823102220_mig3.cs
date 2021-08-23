using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "User",
                newName: "User_last_name");

            migrationBuilder.AddColumn<string>(
                name: "User_first_name",
                table: "User",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User_first_name",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "User_last_name",
                table: "User",
                newName: "Username");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
