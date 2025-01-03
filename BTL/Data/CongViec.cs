﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Data
{
    [Table("CongViec")]
    public class CongViec
    {
        [Key]
        public int MaCV { get; set; }
        [Required]
        [MaxLength(50)]
        public string TenCV{ get; set; }
        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}
