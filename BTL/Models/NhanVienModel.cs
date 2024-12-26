using System.ComponentModel.DataAnnotations;

namespace BTL.Models
{
    public class NhanVienModel
    {
        [Required]
        [MaxLength(100)]
        public string HoTen { get; set; }
        [Required]
        [MaxLength(100)]
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public string NgaySinh { get; set; }
        public int MaCongViec { get; set; }
        public int MaPhongBan { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Luong { get; set; }
        public string Anh { get; set; }
    }
}
