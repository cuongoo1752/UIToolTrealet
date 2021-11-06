using Microsoft.EntityFrameworkCore.Migrations;

namespace UIToolTrealet.Data.Migrations
{
    public partial class initialsetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "imageCodeTrealet",
                table: "Image",
                newName: "ImageCodeTrealet");

            migrationBuilder.AddColumn<string>(
                name: "ImageCodeAvatar",
                table: "Item",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageCodeAvatar",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "ImageCodeTrealet",
                table: "Image",
                newName: "imageCodeTrealet");
        }
    }
}
