using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NhanVien_CongViec_MaCongViec",
                table: "NhanVien");

            migrationBuilder.DropColumn(
                name: "MaCV",
                table: "NhanVien");

            migrationBuilder.AlterColumn<int>(
                name: "MaCongViec",
                table: "NhanVien",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NhanVien_CongViec_MaCongViec",
                table: "NhanVien",
                column: "MaCongViec",
                principalTable: "CongViec",
                principalColumn: "MaCongViec",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NhanVien_CongViec_MaCongViec",
                table: "NhanVien");

            migrationBuilder.AlterColumn<int>(
                name: "MaCongViec",
                table: "NhanVien",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MaCV",
                table: "NhanVien",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_NhanVien_CongViec_MaCongViec",
                table: "NhanVien",
                column: "MaCongViec",
                principalTable: "CongViec",
                principalColumn: "MaCongViec");
        }
    }
}
