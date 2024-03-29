using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanGiay.Models
{
    public class Kho
    {
        //[Key]
        //[Column(Order = 1)]
        //[ForeignKey("iMaNCC")]
        public int iMaNCC { get; set; }

        //[Key]
        //[Column(Order = 2)]
        //[ForeignKey("iMaGiay")]
        public int iMaGiay { get; set; }

        public int iSize { get; set; }
       
        public int iSoLuongKho { get; set; }

        public int iGiaNhap { get; set; }
    }
}
