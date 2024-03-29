using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace QuanLyBanGiay.Models
{
    public class DBCustx:DbContext
    {
        public DBCustx(DbContextOptions<DBCustx> options) : base(options) { }
        //public DbSet<NhanVien> NhanViens { get; set; }

        public DbSet<NhanVien> tblNhanVien { get; set; }
        public DbSet<BoPhan> tblBoPhan { get; set; }
        public DbSet<Giay_Anh> tblGiay_Anh { get; set; }
        public DbSet<NhaCungCap> tblNhaCungCap { get; set; }
        public DbSet <Giay> tblGiay { get; set; }
        public DbSet<Kho> tblKho { get; set; }
        public DbSet<PhieuNhapKho> tblPhieuNhapKho { get; set; }
        public DbSet<ChiTietPhieuNhapKho> tblChiTietPhieuNhapKho { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kho>()
                .HasKey(k => new { k.iMaNCC, k.iMaGiay, k.iSize});

            modelBuilder.Entity<ChiTietPhieuNhapKho>().HasKey(k => new {k.iMaGiay, k.iMaPNK, k.iSize});
        }

       
        //public IQueryable<NhanVien> TimKiemNV(string TenNV)
        //{
        //    SqlParameter pContactName = new SqlParameter("@TenNV", TenNV);

        //    return this.tblNhanVien.FromSqlRaw("EXECUTE TimKiemNV @TenNV", pContactName);

        //}

        //public IQueryable<NhanVien> DSNV()
        //{

        //    return this.tblNhanVien.FromSqlRaw("Select * from tblNhanVien");

        //}


    }
}
