using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Data
{
    [Table("HopDong")]
    public class HopDong
    {
        [Key]
        public string MaHD { get; set; }
        public int MaNV {  get; set; }  
        public string LoaiHD { get; set; }
        public string TuNgay { get; set; }
        public string DenNgay { get; set; }
        [ForeignKey("MaNV")]
        public NhanVien? NhanVien { get; set; }
    }
}
