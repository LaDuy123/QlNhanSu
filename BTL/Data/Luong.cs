using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Data
{
    [Table("Luong")]
    public class Luong
    {
        [Key]
        public string BacLuong {  get; set; }
        public int LuongCB { get; set; }
        public string HSLuong { get; set; }
        public string HSPC { get; set;}
        public virtual ICollection<NhanVien> NhanViens { get; set; }
        public decimal CalculatedTongLuong
        {
            get
            {
                // Tính tổng lương = Lương cơ bản * (Hệ số lương + Hệ số phụ cấp)
                var hesoLuong = decimal.TryParse(HSLuong, out var hsLuong) ? hsLuong : 0;
                var hesoPhuCap = decimal.TryParse(HSPC, out var hsPhuCap) ? hsPhuCap : 0;

                return LuongCB * (1 + hesoLuong + hesoPhuCap);
            }
        }
        public decimal TongLuong { get; set; }
    }
}
