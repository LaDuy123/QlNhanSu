using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Data
{
    [Table("TrinhDoHocVan")]
    public class TrinhDoHocVan
    {
        [Key]
        public int MaTDHV {  get; set; }
        public string TenTDHV { get; set; }
        public string CNganh { get; set; }
        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}
