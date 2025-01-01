
namespace BTL.Dto
{
    public class NhanVienCreateDto
    {
        public string HoTen { get; set; }
        public string QueQuan { get; set; }
        public string GioiTinh { get; set; }
        public string DienThoai { get; set; }
        public string DanToc { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string NgaySinh { get; set; }
        public int MaCV { get; set; }
        public int MaPB { get; set; }
        public int MaTDHV { get; set; }
        public string BacLuong { get; set; }
        public IFormFile Anh { get; set; }

    }
}
