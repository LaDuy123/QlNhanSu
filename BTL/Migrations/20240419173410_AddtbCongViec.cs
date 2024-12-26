using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL.Migrations
{
    public partial class AddtbCongViec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CongViec",
                columns: table => new
                {
                    MaCV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenCV = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CongViec", x => x.MaCV);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NhanVien_CongViec_MaCV",
                table: "NhanVien");

            migrationBuilder.DropTable(
                name: "CongViec");

            migrationBuilder.DropIndex(
                name: "IX_NhanVien_MaCV",
                table: "NhanVien");
        }
    }
}
