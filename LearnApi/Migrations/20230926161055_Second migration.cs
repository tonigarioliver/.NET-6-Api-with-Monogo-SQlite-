using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnApi.Migrations
{
    /// <inheritdoc />
    public partial class Secondmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", unicode: false, maxLength: 250, nullable: false),
                    email = table.Column<string>(type: "TEXT", unicode: false, maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "TEXT", unicode: false, maxLength: 20, nullable: true),
                    password = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    isactive = table.Column<bool>(type: "INTEGER", nullable: true),
                    role = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    tokenid = table.Column<int>(type: "INTEGER", unicode: false, nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    refreshtoken = table.Column<string>(type: "TEXT", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.tokenid);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_tokenid",
                        column: x => x.tokenid,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                table: "Users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
