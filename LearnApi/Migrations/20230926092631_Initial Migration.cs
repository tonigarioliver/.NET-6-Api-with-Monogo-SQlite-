using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    Creditlimit = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: true),
                    Taxcode = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
