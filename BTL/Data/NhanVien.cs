﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Data
{
    [Table("NhanVien")]
    public class NhanVien
    {
        [Key]
        public int MaNV { get; set; }
        [Required]
        [MaxLength(100)]
        public string HoTen { get; set; }
        [Required]
        [MaxLength(100)]
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
        public string Anh { get; set; }
        [ForeignKey("MaCV")]
        public CongViec? CongViec { get; set; }
        [ForeignKey("MaPB")]
        public PhongBan? PhongBan { get; set; }
        [ForeignKey("MaTDHV")]
        public TrinhDoHocVan? TrinhDoHocVan { get; set; }
        [ForeignKey("BacLuong")]
        public Luong? luong { get; set; }
    }
}
