using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.EntityFrameworkCore;
using QuanLyBanGiay.Models;
using System.Diagnostics;
using System.Web;

using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting.Server;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using System.Net.WebSockets;
using static System.Net.Mime.MediaTypeNames;
using System;


namespace QuanLyBanGiay.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DBCustx _dbCustx { get; }

        private readonly IMemoryCache _memoryCache;


        public AdminController(ILogger<HomeController> logger, DBCustx dBCustx, IMemoryCache memoryCache)
        {
            _logger = logger;
            this._dbCustx = dBCustx;
            _memoryCache = memoryCache;
        }

        public async Task<IActionResult> MemoryCache()
        {
            var cacheData = _memoryCache.Get<IEnumerable<BoPhan>>("tblBoPhan");
            if (cacheData != null)
            {
                return View(cacheData);
            }
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            cacheData = await _dbCustx.tblBoPhan.ToListAsync();
            _memoryCache.Set("tblBoPhan", cacheData, expirationTime);
            return View(cacheData);
        }

        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult NhaCungCap()
        {
            var lstNCC = _dbCustx.tblNhaCungCap.ToList();
            return View(lstNCC);
        }

        [HttpPost]
        public IActionResult NhaCungCap(string TenNCC)
        {
            if (TenNCC == null)
            {
                TenNCC = " ";
                var a = _dbCustx.tblNhaCungCap.ToList();
                var lstNCC = a.Where(item => item.sTenNCC.ToLower().Contains(TenNCC.ToLower()) == true).ToList();
                return View(lstNCC);
            }
            else
            {
                var a = _dbCustx.tblNhaCungCap.ToList();
                var lstNCC = a.Where(item => item.sTenNCC.ToLower().Contains(TenNCC.ToLower()) == true).ToList();
                return View(lstNCC);
            }

        }

        public IActionResult NhaCungCap_ThemMoi()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NhaCungCap_ThemMoi(string MaNCC, string TenNCC, string DiaChi, string SDT)
        {
            NhaCungCap nhaCungCap = new NhaCungCap();
            nhaCungCap.iMaNCC = Convert.ToInt16(MaNCC);
            nhaCungCap.sTenNCC = TenNCC;
            nhaCungCap.sDiaChi = DiaChi;
            nhaCungCap.sSDT = SDT;

            _dbCustx.tblNhaCungCap.Add(nhaCungCap);
            _dbCustx.SaveChanges();
            return RedirectToAction("NhaCungCap");
        }

        [HttpPost]
        public IActionResult NhaCungCap_Xoa(string MaNCC)
        {
            ViewData["test"] = MaNCC;
            var ncc = _dbCustx.tblNhaCungCap.SingleOrDefault(item => item.iMaNCC.Equals(Convert.ToInt16(MaNCC)) == true);
            _dbCustx.tblNhaCungCap.Remove(ncc);
            _dbCustx.SaveChanges();
            return RedirectToAction("NhaCungCap");

        }


        public IActionResult NhaCungCap_Sua(string id)
        {

            ViewBag.MaNCC = id;

            return View();
        }

        [HttpPost]
        public IActionResult NhaCungCap_Sua_Handle(string MaNCC, string TenNCC, string DiaChi, string SDT)
        {
            var ncc = _dbCustx.tblNhaCungCap.SingleOrDefault(item => item.iMaNCC.Equals(Convert.ToInt16(MaNCC)) == true);
            ncc.sTenNCC = TenNCC;
            ncc.sDiaChi = DiaChi;
            ncc.sSDT = SDT;
            _dbCustx.SaveChanges();
            return RedirectToAction("NhaCungCap");
        }


        public IActionResult BoPhan()
        {
            var lstBP = _dbCustx.tblBoPhan.ToList();
            return View(lstBP);
        }

        [HttpPost]
        public IActionResult BoPhan(string TenBP)
        {
            if (TenBP == null)
            {
                TenBP = " ";
                var a = _dbCustx.tblBoPhan.ToList();
                var lstBP = a.Where(item => item.sTenBP.ToLower().Contains(TenBP.ToLower()) == true).ToList();
                return View(lstBP);
            }
            else
            {
                var a = _dbCustx.tblBoPhan.ToList();
                var lstBP = a.Where(item => item.sTenBP.ToLower().Contains(TenBP.ToLower()) == true).ToList();
                return View(lstBP);
            }


        }

        public IActionResult BoPhan_ThemMoi()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BoPhan_ThemMoi(string MaBP, string TenBP)
        {

            BoPhan bophan = new BoPhan();
            bophan.iMaBP = Convert.ToInt16(MaBP);
            bophan.sTenBP = TenBP;
            _dbCustx.tblBoPhan.Add(bophan);
            _dbCustx.SaveChanges();
            return RedirectToAction("BoPhan");
        }

        //public IActionResult BoPhan_Xoa()
        //{

        //    return View();
        //}

        [HttpPost]
        public IActionResult BoPhan_Xoa(string MaBP)
        {
            var hasEmployees = _dbCustx.tblNhanVien.Any(nv => nv.iMaBP == Convert.ToInt16(MaBP));
            if (hasEmployees)
            {
                // Nếu có nhân viên liên kết, hiển thị thông báo và không thực hiện xóa
                TempData["ErrorMessage"] = "Không thể xóa bộ phận này vì có nhân viên liên kết.";
                return RedirectToAction("BoPhan");
            }


            var bp = _dbCustx.tblBoPhan.SingleOrDefault(item => item.iMaBP.Equals(Convert.ToInt16(MaBP)) == true);
            _dbCustx.tblBoPhan.Remove(bp);
            _dbCustx.SaveChanges();
            return RedirectToAction("BoPhan");
        }

        public IActionResult BoPhan_Sua(string id)
        {

            ViewBag.MaBP = id;

            return View();
        }

        [HttpPost]
        public IActionResult BoPhan_Sua_Handle(string MaBP, string TenBP)
        {
            var bp = _dbCustx.tblBoPhan.SingleOrDefault(item => item.iMaBP.Equals(Convert.ToInt16(MaBP)) == true);
            bp.sTenBP = TenBP;
            _dbCustx.SaveChanges();
            return RedirectToAction("BoPhan");
        }

        public IActionResult Kho()
        {
            var lstKho = _dbCustx.tblKho.ToList();
            Console.WriteLine(lstKho);
            return View(lstKho);

        }

        public IActionResult Kho_ThemMoi()
        {
            return View();
        }

        // Xử lý thêm mới Kho
        [HttpPost]
        public IActionResult Kho_ThemMoi(int MaNCC, int MaGiay, int SoLuongKho, int GiaNhap, int Size)
        {
            Kho kho = new Kho();
            kho.iMaNCC = MaNCC;
            kho.iMaGiay = MaGiay;
            kho.iSize = Convert.ToInt32(Size.ToString());
            kho.iSoLuongKho = SoLuongKho;
            kho.iGiaNhap = GiaNhap;
            _dbCustx.tblKho.Add(kho);
            _dbCustx.SaveChanges();
            return RedirectToAction("Kho");
        }

        // Xóa Kho
        [HttpPost]
        public IActionResult Kho_Xoa(int MaNCC, int MaGiay)
        {
            var kho = _dbCustx.tblKho.FirstOrDefault(item => item.iMaNCC.Equals(MaNCC) && item.iMaGiay.Equals(MaGiay));
            _dbCustx.tblKho.Remove(kho);
            _dbCustx.SaveChanges();
            return RedirectToAction("Kho");
        }

        // Hiển thị form sửa Kho
        public IActionResult Kho_Sua(int MaNCC, int MaGiay)
        {
            ViewBag.MaNCC = MaNCC;
            ViewBag.MaGiay = MaGiay;
            return View();
        }

        // Xử lý sửa Kho
        [HttpPost]
        public IActionResult Kho_Sua_Handle(int MaNCC, int MaGiay, int SoLuongKho, int GiaNhap, int Size)
        {
            var kho = _dbCustx.tblKho.SingleOrDefault(item => item.iMaNCC.Equals(MaNCC) && item.iMaGiay.Equals(MaGiay));
            kho.iSoLuongKho = SoLuongKho;
            kho.iGiaNhap = GiaNhap;
            kho.iSize = Size;
            _dbCustx.SaveChanges();
            return RedirectToAction("Kho");
        }


        public IActionResult Giay()
        {
            var query = from giay in _dbCustx.tblGiay.ToList()
                        join ncc in _dbCustx.tblNhaCungCap.ToList() on giay.iMaNCC equals ncc.iMaNCC
                        select new
                        {
                            MaGiay = giay.iMaGiay,
                            TenGiay = giay.sTenGiay,
                            Gia = giay.iGia,
                            GiaKM = giay.iGiaKM,
                            NCC = ncc.sTenNCC,
                            Anh = giay.sImageUrl
                        };

            var results = query.ToList();
            return View(results);

            //var lstGiay = _dbCustx.tblGiay.ToList();
            //return View(lstGiay);
        }

        [HttpPost]
        public IActionResult Giay(string TenGiay)
        {
            if (TenGiay == null)
            {
                TenGiay = " ";
                var a = _dbCustx.tblGiay.ToList();
                var lstGiay = a.Where(item => item.sTenGiay.ToLower().Contains(TenGiay.ToLower()) == true).ToList();
                return View(lstGiay);
            }
            else
            {
                var a = _dbCustx.tblGiay.ToList();
                var lstGiay = a.Where(item => item.sTenGiay.ToLower().Contains(TenGiay.ToLower()) == true).ToList();
                return View(lstGiay);
            }


        }

        public IActionResult Giay_ThemMoi()
        {
            var a = _dbCustx.tblGiay.ToList();
            return View(a);
        }
        [HttpPost]
        public async Task<IActionResult> Giay_ThemMoi(IFormFile Anh, string MaGiay, string TenGiay, string MaNCC, string Gia, List<string> SoLuong, List<string> Size, List<string> GiaNhap, string MaLG, string GiaKM, string Mota)
        {
            if (Anh != null && Anh.Length > 0)
            {
                var fileName = Path.GetFileName(Anh.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Anh.CopyToAsync(fileStream);
                }

                ViewBag.Message = "File uploaded successfully";
                var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}";
                var fileUrl = "~/image/" + fileName;
                ViewData["files"] = fileUrl;

                Giay giay = new Giay();
                giay.iMaGiay = Convert.ToInt32(MaGiay);
                giay.sTenGiay = TenGiay;
                giay.iGia = Convert.ToInt32(Gia);
                giay.iMaNCC = Convert.ToInt32(MaNCC);
                giay.iMaLoaiGiay = Convert.ToInt16(MaLG);
                giay.sImageUrl = fileUrl;
                giay.iGiaKM = Convert.ToInt32(GiaKM);
                giay.sMota = Mota;

                _dbCustx.tblGiay.Add(giay);
                _dbCustx.SaveChanges();



                if (Size != null && SoLuong != null && GiaNhap != null)
                {
                    for (int i = 0; i < Size.Count; i++)
                    {
                        int giaNhapValue;
                        Int32.TryParse(GiaNhap[i], out giaNhapValue);
                        Kho kho = new Kho();
                        kho.iMaNCC = Convert.ToInt32(MaNCC);
                        kho.iMaGiay = Convert.ToInt32(MaGiay);
                        kho.iSize = Convert.ToInt32(Size[i]);
                        kho.iSoLuongKho = Convert.ToInt32(SoLuong[i]);
                        kho.iGiaNhap = Convert.ToInt32(giaNhapValue);

                        // Thêm đối tượng KichThuocGiay vào DbSet và lưu vào cơ sở dữ liệu
                        _dbCustx.tblKho.Add(kho);
                        _dbCustx.SaveChanges();
                    }
                }

            }
            return RedirectToAction("Giay");

        }

        [HttpPost]
        public IActionResult Giay_Xoa(string MaGiay)
        {
            var TimKho = _dbCustx.tblKho.Any(nv => nv.iMaGiay == Convert.ToInt16(MaGiay));
            if (TimKho)
            {
                // Nếu có nhân viên liên kết, hiển thị thông báo và không thực hiện xóa
                TempData["ErrorMessage"] = "Không thể xóa giày này vì có dữ liệu liên kết trong bảng tblKho.";
                return RedirectToAction("Giay");
            }


            var giay = _dbCustx.tblGiay.SingleOrDefault(item => item.iMaGiay.Equals(Convert.ToInt16(MaGiay)) == true);
            _dbCustx.tblGiay.Remove(giay);
            _dbCustx.SaveChanges();
            return RedirectToAction("Giay");
        }

        public IActionResult Giay_Sua(int MaGiay)
        {
            ViewBag.MaGiay = MaGiay;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Giay_Sua_Handle(IFormFile Anh, string MaGiay, string TenGiay, string MaNCC, string Gia, string MaLG, string GiaKM, string Mota)
        {
            var fileName = Path.GetFileName(Anh.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await Anh.CopyToAsync(fileStream);
            }

            ViewBag.Message = "File uploaded successfully";
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}";
            var fileUrl = "~/image/" + fileName;
            ViewData["files"] = fileUrl;

            var giay = _dbCustx.tblGiay.SingleOrDefault(item => item.iMaGiay == Convert.ToInt32(MaGiay));
            giay.iMaGiay = Convert.ToInt32(MaGiay);
            giay.sTenGiay = TenGiay;
            giay.iGia = Convert.ToInt32(Gia);
            giay.iMaNCC = Convert.ToInt32(MaNCC);
            giay.iMaLoaiGiay = Convert.ToInt16(MaLG);
            giay.sImageUrl = fileUrl;
            giay.iGiaKM = Convert.ToInt32(GiaKM);
            giay.sMota = Mota;
            _dbCustx.SaveChanges();
            return RedirectToAction("Giay");
        }


        public IActionResult NhanVien()
        {
            var query = from nv in _dbCustx.tblNhanVien.ToList()
                        join bp in _dbCustx.tblBoPhan.ToList() on nv.iMaBP equals bp.iMaBP
                        select new
                        {
                            TenNhanVien = nv.sTenNV,
                            TenBoPhan = bp.sTenBP
                        };

            var results = query.ToList();
            return View(results);
            //var lstNV = _dbCustx.tblNhanVien.ToList();

            //return View(lstNV);
        }
        [HttpPost]
        public IActionResult NhanVien(string TenNV)
        {

            var a = _dbCustx.tblNhanVien.ToList();
            var lstNV = a.Where(item => item.sTenNV.ToLower().Contains(TenNV.ToLower()) == true).ToList();
            return View(lstNV);

        }
        public IActionResult NhanVien_ThemMoi()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NhanVien_ThemMoi(int MaNV, string TenNV,DateTime NgaySinh,string GioiTinh, string QueQuan, string SDT, int LCB,float HSL,int PC,DateTime NgayVaoLam,int MaBP)
        {
            NhanVien nhanVien = new NhanVien
            {
                iMaNV = MaNV,
                sTenNV = TenNV,
                dNgaysinh = NgaySinh,
                sGioiTinh = GioiTinh,
                sQueQuan = QueQuan,
                sSDT = SDT,
                iLuongCB = LCB,
                fHSL = HSL,
                iPC = PC,
                iMaBP = MaBP,
                dNgayvaolam = NgayVaoLam,
            };

            _dbCustx.tblNhanVien.Add(nhanVien);
            _dbCustx.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult NhanVien_Xoa(int id)
        {
            var nv = _dbCustx.tblNhanVien.Find(id);
            if (nv != null)
            {
                _dbCustx.tblNhanVien.Remove(nv);
                _dbCustx.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult NhanVien_Sua(int id)
        {
            var nv = _dbCustx.tblNhanVien.Find(id);
            if (nv == null)
            {
                return NotFound();
            }
            return View(nv);
        }

        [HttpPost]
        public IActionResult NhanVien_Sua_Handle(int MaNV, string TenNV, DateTime NgaySinh, string GioiTinh, string QueQuan, string SDT, int LCB, float HSL, int PC, DateTime NgayVaoLam, int MaBP)
        {
            var nv = _dbCustx.tblNhanVien.Find(MaNV);
            if (nv == null)
            {
                return NotFound();
            }

            nv.sTenNV = TenNV;
            nv.dNgaysinh = NgaySinh;
            nv.sGioiTinh = GioiTinh;
            nv.sQueQuan = QueQuan;
            nv.sSDT = SDT;
            nv.iLuongCB = LCB;
            nv.fHSL = HSL;
            nv.iPC = PC;
            nv.iMaBP = MaBP;
            nv.dNgayvaolam = NgayVaoLam;

            _dbCustx.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult LuuFile()
        {
            var a = _dbCustx.tblGiay_Anh.ToList();
            return View(a);

        }

        public IActionResult XemAnh()
        {
            var a = _dbCustx.tblGiay_Anh.ToList();
            return View(a);

        }

        [HttpPost]
        public async Task<IActionResult> LuuFile(IFormFile fileupload, string ten)
        {
            if (fileupload != null && fileupload.Length > 0)
            {
                var fileName = Path.GetFileName(fileupload.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await fileupload.CopyToAsync(fileStream);
                }

                ViewBag.Message = "File uploaded successfully";
                var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}";
                var fileUrl = "~/image/" + fileName;
                ViewData["files"] = fileUrl;

                var giayAnh = new Giay_Anh();
                giayAnh.sMaGiay = ten;
                giayAnh.sImageUrl = fileUrl;

                // Thêm vào DbContext và lưu vào cơ sở dữ liệu
                _dbCustx.tblGiay_Anh.Add(giayAnh);
                _dbCustx.SaveChanges();

            }
            return View();
        }

        public IActionResult DonHang()
        {
            return View();
        }



        public IActionResult KhachHang()
        {
            var lstKH = _dbCustx.tblKhachHang.ToList();
            return View(lstKH);

        }

        [HttpPost]
        public IActionResult KhachHang(string TenKH)
        {
            if (TenKH == null)
            {
                TenKH = " ";
            }

            var lstKH = _dbCustx.tblKhachHang
                                .Where(item => item.sTenKH.ToLower().Contains(TenKH.ToLower()))
                                .ToList();

            return View(lstKH);
        }

        public IActionResult KhachHang_ThemMoi()
        {
            return View();
        }

        [HttpPost]
        public IActionResult KhachHang_ThemMoi(int MaKH, string TenKH, string DiaChi, string SDT, int MaTK)
        {
            KhachHang khachHang = new KhachHang
            {
                iMaKH = MaKH,
                sTenKH = TenKH,
                sDiaChi = DiaChi,
                sSDT = SDT,
                iMaTK = MaTK
            };

            _dbCustx.tblKhachHang.Add(khachHang);
            _dbCustx.SaveChanges();
            return RedirectToAction("KhachHang");
        }

        [HttpPost]
        public IActionResult KhachHang_Xoa(int id)
        {
            var kh = _dbCustx.tblKhachHang.Find(id);
            if (kh != null)
            {
                _dbCustx.tblKhachHang.Remove(kh);
                _dbCustx.SaveChanges();
            }
            return RedirectToAction("KhachHang");
        }

        public IActionResult KhachHang_Sua(int id)
        {
            @ViewBag.MaKH = id;
            return View();
        }

        [HttpPost]
        public IActionResult KhachHang_Sua_Handle(string MaKH, string TenKH, string DiaChi, string SDT,int MaTK)
        {
            var kh = _dbCustx.tblKhachHang.SingleOrDefault(item => item.iMaKH.Equals(Convert.ToInt16(MaKH)) == true);
            kh.sTenKH = TenKH;
            kh.sDiaChi = DiaChi;
            kh.sSDT = SDT;
            kh.iMaTK = MaTK;

            _dbCustx.SaveChanges();
            return RedirectToAction("KhachHang");
        }



        public IActionResult LoaiGiay()
        {
            var lstLG = _dbCustx.tblLoaiGiay.ToList(); 
            return View(lstLG);
        }

        [HttpPost]
        public IActionResult LoaiGiay(string TenLoaiGiay)
        {
            if (TenLoaiGiay == null)
            {
                TenLoaiGiay = " ";
            }

            var lstLG = _dbCustx.tblLoaiGiay
                                .Where(item => item.sTenLoaiGiay.ToLower().Contains(TenLoaiGiay.ToLower()))
                                .ToList();

            return View(lstLG);
        }

        public IActionResult LoaiGiay_ThemMoi()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoaiGiay_ThemMoi(int MaLoaiGiay, string TenLoaiGiay)
        {
            LoaiGiay loaiGiay = new LoaiGiay
            {
                iMaLoaiGiay = MaLoaiGiay,
                sTenLoaiGiay = TenLoaiGiay
            };

            _dbCustx.tblLoaiGiay.Add(loaiGiay);
            _dbCustx.SaveChanges();
            return RedirectToAction("LoaiGiay");
        }

        [HttpPost]
        public IActionResult LoaiGiay_Xoa(string MaLoaiGiay)
        {
            ViewData["test"] = MaLoaiGiay;
            var lg = _dbCustx.tblLoaiGiay.SingleOrDefault(item => item.iMaLoaiGiay.Equals(Convert.ToInt16(MaLoaiGiay)) == true);
            _dbCustx.tblLoaiGiay.Remove(lg);
            _dbCustx.SaveChanges();
            return RedirectToAction("LoaiGiay");
        }
       

        public IActionResult LoaiGiay_Sua(string id)
        {
            ViewBag.MaLoaiGiay = id;
            return View();
        }

        [HttpPost]
        public IActionResult LoaiGiay_Sua_Handle(string MaLoaiGiay, string TenLoaiGiay)
        {
            var lg = _dbCustx.tblLoaiGiay.SingleOrDefault(item => item.iMaLoaiGiay.Equals(Convert.ToInt16(MaLoaiGiay))==true);
            lg.sTenLoaiGiay = TenLoaiGiay;
            _dbCustx.SaveChanges();
            return RedirectToAction("LoaiGiay");
        }

        public IActionResult TaiKhoan()
        {
            var lstTaiKhoan = _dbCustx.tblTaiKhoan.ToList();
            return View(lstTaiKhoan);
        }

        [HttpPost]
        public IActionResult TaiKhoan(string TenTaiKhoan)
        {
            if (TenTaiKhoan == null)
            {
                TenTaiKhoan = " ";
            }

            var lstTaiKhoan = _dbCustx.tblTaiKhoan
                                .Where(item => item.sTenTK.ToLower().Contains(TenTaiKhoan.ToLower()))
                                .ToList();

            return View(lstTaiKhoan);
        }

        public IActionResult TaiKhoan_ThemMoi()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TaiKhoan_ThemMoi(int MaTK,string TenTK, string MatKhau)
        {
            TaiKhoan taiKhoan = new TaiKhoan
            {
                iMaTK = MaTK,
                sTenTK = TenTK,
                sMatkhau = MatKhau
            };

            _dbCustx.tblTaiKhoan.Add(taiKhoan);
            _dbCustx.SaveChanges();
            return RedirectToAction("TaiKhoan");
        }

        [HttpPost]
        public IActionResult TaiKhoan_Xoa(string MaTK)
        {
            ViewData["test"] = MaTK;
            var tk = _dbCustx.tblTaiKhoan.SingleOrDefault(item => item.iMaTK.Equals(Convert.ToInt16(MaTK)) == true);
            _dbCustx.tblTaiKhoan.Remove(tk);
            _dbCustx.SaveChanges();
            return RedirectToAction("TaiKhoan");
        }

        public IActionResult TaiKhoan_Sua(int id)
        {
            ViewBag.MaTK = id;
            return View();
        }

        [HttpPost]
        public IActionResult TaiKhoan_Sua_Handle(string MaTK, string TenTaiKhoan, string MatKhau)
        {
            var tk = _dbCustx.tblTaiKhoan.SingleOrDefault(item => item.iMaTK.Equals(Convert.ToInt16(MaTK)) == true);
            tk.sTenTK = TenTaiKhoan;
            tk.sMatkhau= MatKhau;
            _dbCustx.SaveChanges();
            return RedirectToAction("TaiKhoan");
        }

        public IActionResult HoaDon()
        {
            var lstHoaDon = _dbCustx.tblHoaDon.ToList();
            return View(lstHoaDon);
        }

        [HttpPost]
        public IActionResult HoaDon(int MaHD)
        {
            var lstHoaDon = _dbCustx.tblHoaDon
                                .Where(item => item.iMaHD.Equals(Convert.ToInt16(MaHD)) == true)
                                .ToList();

            return View(lstHoaDon);
        }

        public IActionResult HoaDon_ThemMoi()
        {
            return View();
        }

        [HttpPost]
        public IActionResult HoaDon_ThemMoi(int MaHD, int MaNV, int MaKH, DateTime NgayLap)
        {
            HoaDon hoaDon = new HoaDon
            {
                iMaHD = MaHD,
                iMaNV = MaNV,
                iMaKH = MaKH,
                dNgayLap = NgayLap
            };

            _dbCustx.tblHoaDon.Add(hoaDon);
            _dbCustx.SaveChanges();
            return RedirectToAction("HoaDon");
        }

        [HttpPost]
        public IActionResult HoaDon_Xoa(int MaHD)
        {
            var hd = _dbCustx.tblHoaDon.SingleOrDefault(item => item.iMaHD.Equals(MaHD));
            _dbCustx.tblHoaDon.Remove(hd);
            _dbCustx.SaveChanges();
            return RedirectToAction("HoaDon");
        }

        public IActionResult HoaDon_Sua(int id)
        {
            ViewBag.MaHD = id;
            return View();
        }

        [HttpPost]
        public IActionResult HoaDon_Sua_Handle(int MaHD, int MaNV, int MaKH, DateTime NgayLap)
        {
            var hd = _dbCustx.tblHoaDon.SingleOrDefault(item => item.iMaHD.Equals(MaHD));
            hd.iMaNV = MaNV;
            hd.iMaKH = MaKH;
            hd.dNgayLap = NgayLap;
            _dbCustx.SaveChanges();
            return RedirectToAction("HoaDon");
        }

        public IActionResult ChiTietHD()
        {
            var lstChiTietHD = _dbCustx.tblChiTietHD.ToList();
            return View(lstChiTietHD);
        }
       


        [HttpPost]
        public IActionResult ChiTietHD(int MaHD)
        {
            var lstChiTietHD = _dbCustx.tblChiTietHD
                                .Where(item => item.iMaHD.Equals(Convert.ToInt16(MaHD)) == true)
                                .ToList();

            return View(lstChiTietHD);
        }

        public IActionResult ChiTietHD_ThemMoi()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChiTietHD_ThemMoi(int MaHD, int MaGiay, int Sl, int Tien)
        {
            ChiTietHD chiTietHD = new ChiTietHD
            {
                iMaHD = MaHD,
                iMaGiay = MaGiay,
                iSl = Sl,
                iTien = Tien
            };

            _dbCustx.tblChiTietHD.Add(chiTietHD);
            _dbCustx.SaveChanges();
            return RedirectToAction("ChiTietHD");
        }

        [HttpPost]
        public IActionResult ChiTietHD_Xoa(int MaHD, int MaGiay)
        {
            var chiTietHD = _dbCustx.tblChiTietHD.FirstOrDefault(item => item.iMaHD == MaHD && item.iMaGiay == MaGiay);
            if (chiTietHD != null)
            {
                _dbCustx.tblChiTietHD.Remove(chiTietHD);
                _dbCustx.SaveChanges();
            }
            return RedirectToAction("ChiTietHD");
        }

        public IActionResult ChiTietHD_Sua(int MaHD, int MaGiay)
        {
            var chiTietHD = _dbCustx.tblChiTietHD.FirstOrDefault(item => item.iMaHD == MaHD && item.iMaGiay == MaGiay);
            if (chiTietHD == null)
            {
                // Handle error - item not found
                return RedirectToAction("ChiTietHD");
            }

            return View(chiTietHD);
        }

        [HttpPost]
        public IActionResult ChiTietHD_Sua_Handle(int MaHD, int MaGiay, int Sl, int Tien)
        {
            var chiTietHD = _dbCustx.tblChiTietHD.FirstOrDefault(item => item.iMaHD == MaHD && item.iMaGiay == MaGiay);
            if (chiTietHD != null)
            {
                chiTietHD.iSl = Sl;
                chiTietHD.iTien = Tien;
                _dbCustx.SaveChanges();
            }
            return RedirectToAction("ChiTietHD");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

