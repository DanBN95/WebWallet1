using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class mog23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Addrees = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    lng = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountBranch",
                columns: table => new
                {
                    AccountsId = table.Column<int>(type: "int", nullable: false),
                    BranchListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountBranch", x => new { x.AccountsId, x.BranchListId });
                    table.ForeignKey(
                        name: "FK_AccountBranch_Account_AccountsId",
                        column: x => x.AccountsId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountBranch_Branch_BranchListId",
                        column: x => x.BranchListId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountBranch_BranchListId",
                table: "AccountBranch",
                column: "BranchListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountBranch");

            migrationBuilder.DropTable(
                name: "Branch");
        }
    }
}
