using Microsoft.EntityFrameworkCore.Migrations;

namespace UIToolTrealet.Data.Migrations
{
    public partial class initialsetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Page_PageId",
                table: "Item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Page",
                table: "Page");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Page");

            migrationBuilder.AddColumn<int>(
                name: "PageId",
                table: "Page",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "PageId",
                table: "Item",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Page",
                table: "Page",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Page_PageId",
                table: "Item",
                column: "PageId",
                principalTable: "Page",
                principalColumn: "PageId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Page_PageId",
                table: "Item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Page",
                table: "Page");

            migrationBuilder.DropColumn(
                name: "PageId",
                table: "Page");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Page",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "PageId",
                table: "Item",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Page",
                table: "Page",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Page_PageId",
                table: "Item",
                column: "PageId",
                principalTable: "Page",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
