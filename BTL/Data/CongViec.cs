using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Data
{
    [Table("CongViec")]
    public class CongViec
    {
        [Key]
        public int MaCongViec { get; set; }
        [Required]
        [MaxLength(50)]
        public string TenCongViec { get; set; }
        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}
