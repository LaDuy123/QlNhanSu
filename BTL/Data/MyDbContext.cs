using Microsoft.EntityFrameworkCore;

namespace BTL.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<CongViec> CongViecs { get; set; }
        public virtual DbSet<PhongBan> PhongBans { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }


    }
}
