using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanGiay.Models
{
    public class PhieuNhapKho
    {
        [Key]
        public int iMaPNK { get; set; }

        [Required]
        [ForeignKey("NhanVien")]
        public int iMaNV { get; set; }

        [Required]
        [ForeignKey("NhaCungCap")]
        public int iMaNCC { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dNgayLap { get; set; }

    }
}
