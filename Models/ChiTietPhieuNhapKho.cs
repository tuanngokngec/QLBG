using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanGiay.Models
{
    public class ChiTietPhieuNhapKho
    {
        [Key, Column(Order = 1)]
        [ForeignKey("PhieuNhapKho")]
        public int iMaPNK { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("Giay")]
        public int iMaGiay { get; set; }

        public int iSlNhap { get; set; }

        public int iGiaNhap { get; set; }

    }
}
