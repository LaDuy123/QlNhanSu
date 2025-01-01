using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Data
{
    [Table("PhongBan")]
    public class PhongBan
    {
        [Key]
        public int MaPB { get; set; }
        [Required]
        [MaxLength(100)]
        public string TenPB { get; set; }
        [Required]
        [MaxLength(100)]
        public string DiaChi { get; set; }
        public string SDTPB { get; set; }
        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}
