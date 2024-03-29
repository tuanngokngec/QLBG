using Microsoft.AspNetCore.Mvc;
using QuanLyBanGiay.Models;
using System.Diagnostics;

namespace QuanLyBanGiay.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DBCustx _dbCustx { get; }

        public HomeController(ILogger<HomeController> logger, DBCustx dBCustx)
        {
            _logger = logger;
            _dbCustx = dBCustx;
        }

        public IActionResult Index()
        {
            //var a = _dbCustx.tblGiay.ToList();
            //var firstFourGiay = a.Take(4).ToList();
            //return View(firstFourGiay);
            var allGiay = _dbCustx.tblGiay.ToList();
            var firstFourGiay = allGiay.Take(4).ToList();
            var remainingGiay = allGiay.Skip(4).Take(4).ToList();

            var giaynu = allGiay.Skip(8).Take(4).Where(item =>item.iMaLoaiGiay == 2);

            ViewBag.FirstFourGiay = firstFourGiay;
            ViewBag.RemainingGiay = remainingGiay;
            ViewBag.GiayNu = giaynu;

            return View();
        }

        public IActionResult GiamGia()
        {
            var allGiay = _dbCustx.tblGiay.ToList();
            var giaynam = allGiay.Where(item => item.iMaLoaiGiay == 1 && item.iGiaKM < item.iGia).ToList();
            var giaynu = allGiay.Where(item => item.iMaLoaiGiay == 2 && item.iGiaKM < item.iGia).ToList();

            ViewBag.GiayNam = giaynam;
            ViewBag.GiayNu = giaynu;

            return View();
        }

        public IActionResult Nam()
        {
            var allGiay = _dbCustx.tblGiay.ToList();
            var giaynam = allGiay.Where(item => item.iMaLoaiGiay == 1).ToList();
            return View(giaynam);
        }

        public IActionResult Nu()
        {
            var allGiay = _dbCustx.tblGiay.ToList();
            var giaynu = allGiay.Where(item => item.iMaLoaiGiay == 2).ToList();
            return View(giaynu);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
