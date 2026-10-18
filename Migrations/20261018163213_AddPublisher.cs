
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costin_Catalina_Lab2.Migrations
{
    public partial class AddPublisher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Publisher",
                columns: table => new
                {
                    ID = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    PublisherName = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publisher", x => x.ID);
                });

            migrationBuilder.AddColumn<int>(
                name: "PublisherID",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_PublisherID",
                table: "Book",
                column: "PublisherID");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Publisher_PublisherID",
                table: "Book",
                column: "PublisherID",
                principalTable: "Publisher",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Publisher_PublisherID",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_PublisherID",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "PublisherID",
                table: "Book");

            migrationBuilder.DropTable(
                name: "Publisher");
        }
    }
}
