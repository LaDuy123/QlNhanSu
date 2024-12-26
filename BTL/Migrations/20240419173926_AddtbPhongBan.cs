using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL.Migrations
{
    public partial class AddtbPhongBan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NhanVien_CongViec_MaCV",
                table: "NhanVien");

            migrationBuilder.DropIndex(
                name: "IX_NhanVien_MaCV",
                table: "NhanVien");

            migrationBuilder.RenameColumn(
                name: "TenCV",
                table: "CongViec",
                newName: "TenCongViec");

            migrationBuilder.RenameColumn(
                name: "MaCV",
                table: "CongViec",
                newName: "MaCongViec");

            migrationBuilder.AddColumn<int>(
                name: "MaCongViec",
                table: "NhanVien",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PhongBan",
                columns: table => new
                {
                    MaPhongBan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhongBan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhongBan", x => x.MaPhongBan);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_MaCongViec",
                table: "NhanVien",
                column: "MaCongViec");

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_MaPhongBan",
                table: "NhanVien",
                column: "MaPhongBan");

            migrationBuilder.AddForeignKey(
                name: "FK_NhanVien_CongViec_MaCongViec",
                table: "NhanVien",
                column: "MaCongViec",
                principalTable: "CongViec",
                principalColumn: "MaCongViec");

            migrationBuilder.AddForeignKey(
                name: "FK_NhanVien_PhongBan_MaPhongBan",
                table: "NhanVien",
                column: "MaPhongBan",
                principalTable: "PhongBan",
                principalColumn: "MaPhongBan",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NhanVien_CongViec_MaCongViec",
                table: "NhanVien");

            migrationBuilder.DropForeignKey(
                name: "FK_NhanVien_PhongBan_MaPhongBan",
                table: "NhanVien");

            migrationBuilder.DropTable(
                name: "PhongBan");

            migrationBuilder.DropIndex(
                name: "IX_NhanVien_MaCongViec",
                table: "NhanVien");

            migrationBuilder.DropIndex(
                name: "IX_NhanVien_MaPhongBan",
                table: "NhanVien");

            migrationBuilder.DropColumn(
                name: "MaCongViec",
                table: "NhanVien");

            migrationBuilder.RenameColumn(
                name: "TenCongViec",
                table: "CongViec",
                newName: "TenCV");

            migrationBuilder.RenameColumn(
                name: "MaCongViec",
                table: "CongViec",
                newName: "MaCV");

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_MaCV",
                table: "NhanVien",
                column: "MaCV");

            migrationBuilder.AddForeignKey(
                name: "FK_NhanVien_CongViec_MaCV",
                table: "NhanVien",
                column: "MaCV",
                principalTable: "CongViec",
                principalColumn: "MaCV",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
