using KK_BookStore.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace KK_BookStore.Controllers
{
    public class AdminController : BaseController
    {
        // GET: Admin
        MyDataDataContext data = new MyDataDataContext();
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            //var soLuongNguoiDung = from tt in data.NguoiDungs select tt;
            //ViewBag.soLuongNguoiDung = soLuongNguoiDung.Count();
            //var soLuongDonHang = from tt in data.HoaDons select tt;
            //ViewBag.soLuongDonHang = soLuongDonHang.Count();
            //var soLuongDonHangChuaDuyet = from tt in data.HoaDons where tt.TinhTrang == null select tt;
            //ViewBag.soLuongDonHangChuaDuyet = soLuongDonHangChuaDuyet.Count();
            //var tongDoanhThu = from tt in data.CTHoaDons select tt.ThanhTien ;
            //ViewBag.tongDoanhThu = tongDoanhThu.Sum();
            //var soLuongNguoiMuaTrongNgay = from tt in data.HoaDons where tt.NgayTao == DateTime.Now select tt;
            //ViewBag.soLuongNguoiMuaTrongNgay = soLuongNguoiMuaTrongNgay.Count();
            //var soLuongNguoiMuaTrongThang = from tt in data.HoaDons where tt.NgayTao.Value.Month == DateTime.Now.Month && tt.NgayTao.Value.Year == DateTime.Now.Year select tt;
            //ViewBag.soLuongNguoiMuaTrongThang = soLuongNguoiMuaTrongThang.Count();
            //var nguoiMuaNhieuNhat = from tt in data.HoaDons orderby tt.TaiKhoan select tt.TaiKhoan;
            //ViewBag.nguoiMuaNhieuNhat = nguoiMuaNhieuNhat.Max();
            //var anhNguoiMuaNhieuNhat = from tt in data.HoaDons orderby tt.TaiKhoan select tt.NguoiDung.Hinh;
            //ViewBag.anhNguoiMuaNhieuNhat = anhNguoiMuaNhieuNhat.Max();

            //var nguoiMuaNhieuNhatTrongThang = from tt in data.HoaDons where tt.NgayTao.Value.Month == DateTime.Now.Month && tt.NgayTao.Value.Year == DateTime.Now.Year orderby tt.TaiKhoan select tt.TaiKhoan;
            //ViewBag.nguoiMuaNhieuNhatTrongThang = nguoiMuaNhieuNhat.Max();
            //var anhNguoiMuaNhieuNhatTrongThang = from tt in data.HoaDons where tt.NgayTao.Value.Month == DateTime.Now.Month && tt.NgayTao.Value.Year == DateTime.Now.Year orderby tt.TaiKhoan select tt.NguoiDung.Hinh;
            //ViewBag.anhNguoiMuaNhieuNhatTrongThang = anhNguoiMuaNhieuNhatTrongThang.Max();

            //var tongDoanhThuTrongThang = from tt in data.CTHoaDons where tt.HoaDon.NgayTao.Value.Month == DateTime.Now.Month && tt.HoaDon.NgayTao.Value.Year == DateTime.Now.Year select tt.ThanhTien;
            //ViewBag.tongDoanhThuTrongThang = tongDoanhThuTrongThang.Sum();

            //var anhSachNhieuLike = from tt in data.Saches  orderby tt.SoLike descending select tt;
            //ViewBag.anhSachNhieuLike = anhSachNhieuLike.First();

            //var tongDoanhThuTrongNgay = from tt in data.CTHoaDons where tt.HoaDon.NgayTao.Value.Day == DateTime.Now.Day && tt.HoaDon.NgayTao.Value.Month == DateTime.Now.Month && tt.HoaDon.NgayTao.Value.Year == DateTime.Now.Year select tt.ThanhTien;
            //ViewBag.tongDoanhThuTrongNgay = tongDoanhThuTrongNgay.Sum();
            var lstThongBao = data.ThongBaos.FirstOrDefault();
            ViewBag.thongBao = lstThongBao;
            return View();
        }
        //BaiViet
        public ActionResult quanLyBaiViet()
        {
            var allPost = from tt in data.BaiViets select tt;
            return View(allPost);
        }


        //User
        public ActionResult quanLyNguoiDung()
        {
            var all_nguoidung = from tt in data.NguoiDungs select tt;

            return View(all_nguoidung);
        }
        public ActionResult camNguoiDung(string id)
        {

            var all_nguoidung = (from tt in data.NguoiDungs where tt.TaiKhoan == id.ToString().Trim() select tt).First();
            if (all_nguoidung.Banned == true)
            {
                all_nguoidung.Banned = false;
                UpdateModel(all_nguoidung);
                data.SubmitChanges();
                return RedirectToAction("quanLyNguoiDung");
            }
            all_nguoidung.Banned = true;
            UpdateModel(all_nguoidung);
            data.SubmitChanges();
            return RedirectToAction("quanLyNguoiDung");
        }
        public ActionResult goCamNguoiDung(string id)
        {

            var all_nguoidung = (from tt in data.NguoiDungs where tt.TaiKhoan == id.Trim() select tt).First();
            all_nguoidung.Banned = false;
            UpdateModel(all_nguoidung);
            data.SubmitChanges();
            return RedirectToAction("quanLyNguoiDung");
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ActionResult EditNguoiDung(string id)
        {
            var D_nguoidung = data.NguoiDungs.Where(m => m.TaiKhoan == id.ToString()).First();
            var D_chucvu = from tt in data.ChucVus select tt;
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var tl in D_chucvu)
                list.Add(new SelectListItem() { Value = tl.MaChucVu.ToString(), Text = tl.TenChucVu });
            ViewBag.chucvu = list;
            return View(D_nguoidung);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public   async Task<ActionResult> EditNguoiDung(string id, NguoiDung model)
        {
            var nguoidung = db.Users.Where(m => m.UserName == id).First();
            var role = db.Roles.Where(m=>m.Id==model.MaChucVu).First();
            var D_chucvu = from tt in data.ChucVus select tt;
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var tl in D_chucvu)
                list.Add(new SelectListItem() { Value = tl.MaChucVu.ToString(), Text = tl.TenChucVu });
            ViewBag.chucvu = list;
            var D_nguoidung = data.NguoiDungs.Where(m => m.TaiKhoan == id.ToString()).First();
            if (D_nguoidung != null)
            {
                D_nguoidung.MaChucVu = model.MaChucVu;
                var result = await UserManager.AddToRoleAsync(nguoidung.Id, role.Name);         
                data.SubmitChanges();              
                return RedirectToAction("quanLyNguoiDung");
            }
            return View();
        }
       



        

        //Categories
        public ActionResult quanLyTheLoai()
        {
            var all_theloai = from tt in data.TheLoais select tt;
            var all_baiviet = from tt in data.BaiViets select tt;
            Dictionary<string, int> categoryCounts = new Dictionary<string, int>();
            
            foreach (var product in all_baiviet)
            {
                
                // Nếu loại sản phẩm đã tồn tại trong dictionary, tăng số lượng lên 1
                if (categoryCounts.ContainsKey(product.TheLoai.TenTL))
                {
                    categoryCounts[product.TheLoai.TenTL]++;
                }
                // Ngược lại, thêm loại sản phẩm mới vào dictionary với số lượng là 1
                else
                {
                    categoryCounts.Add(product.TheLoai.TenTL, 1);
                }
            }
            // Kiểm tra xem có thể loại mới nào chưa có bài viết không
            foreach (var theLoai in all_theloai)
            {
                if (!categoryCounts.ContainsKey(theLoai.TenTL))
                {
                    // Nếu thể loại chưa có bài viết, thêm nó vào dictionary với số lượng là 0
                    categoryCounts.Add(theLoai.TenTL, 0);
                }
            }

            //kiem tra mot the loai ma khong co post nao
            // Lấy mã thể loại và số lượng bài viết
            Dictionary<int, int> maTLCounts = new Dictionary<int, int>();
            foreach (var theLoai in all_theloai)
            {
                int maTL = theLoai.MaTL;
                if (categoryCounts.ContainsKey(theLoai.TenTL))
                {
                    int soLuongBaiViet = categoryCounts[theLoai.TenTL];
                    maTLCounts.Add(maTL, soLuongBaiViet);
                }
            }
            
            ViewBag.CategoryCounts = categoryCounts;
            //ViewBag.CategoryCounts = categoryCounts;
            ViewBag.theloai = all_theloai;
            ViewBag.baiviet = all_baiviet;
            return View(all_theloai);



        }
        public ActionResult themTheLoai()
        {
            return View();
        }
        [HttpPost]
        public ActionResult themTheLoai(TheLoai model)
        {
            data.TheLoais.InsertOnSubmit(model);
            data.SubmitChanges();
            return Redirect("quanLyTheLoai");
        }
        public ActionResult xoaTheLoai(int id)
        {
            var D_theloai = data.TheLoais.Where(m => m.MaTL == id).First();
            data.TheLoais.DeleteOnSubmit(D_theloai);
            data.SubmitChanges();
            return RedirectToAction("quanLyTheLoai");
        }

        public ActionResult EditTheLoai(int id)
        {

            var D_theloai = data.TheLoais.Where(m => m.MaTL == id).FirstOrDefault();

            return View(D_theloai);

        }
        [HttpPost, ActionName("EditTheLoai")]
        [ValidateAntiForgeryToken]
        public ActionResult EditTheLoai(int id, TheLoai model)
        {
            var D_theloai = data.TheLoais.Where(m => m.MaTL == id).FirstOrDefault();

            // Checking if any such record exist
            if (D_theloai != null)
            {
                D_theloai.TenTL = model.TenTL;
                data.SubmitChanges();
                return RedirectToAction("quanLyTheLoai");
            }
            return Redirect("quanLyTheLoai");

        }




        public ActionResult danhSachBaiVietTheoTheLoai(int id, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 3;
            int pageNum = page ?? 1;
           
            var all_baiViet = from tt in data.BaiViets where tt.MaTL == id select tt;
            return View(all_baiViet.ToPagedList(pageNum, pageSize));
        }



        //role
        public ActionResult quanLyVaiTro()
        {
            var all_VaiTro = from tt in data.ChucVus select tt;
            return View(all_VaiTro);
        }
        public ActionResult themVaiTro()
        {
            return View();
        }
        [HttpPost]
        public ActionResult themVaiTro(ChucVu model)
        {
            data.ChucVus.InsertOnSubmit(model);
            data.SubmitChanges();
            return Redirect("quanLyVaiTro");
        }
        //public ActionResult xoaVaiTro(int id)
        //{
        //    var D_chucvu = data.ChucVus.Where(m => m.MaChucVu == id).First();
        //    data.ChucVus.DeleteOnSubmit(D_chucvu);
        //    data.SubmitChanges();
        //    return RedirectToAction("quanLyVaiTro");
        //}
        public ActionResult EditVaiTro(string id)
        {
            var D_vaitro = data.ChucVus.Where(m => m.MaChucVu == id).First();
            return View(D_vaitro);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVaiTro(string id, ChucVu model)
        {
            var D_vaitro = data.ChucVus.Where(m => m.MaChucVu == id).First();

            // Checking if any such record exist
            if (D_vaitro != null)
            {
                D_vaitro.TenChucVu = model.TenChucVu;
                data.SubmitChanges();
                return RedirectToAction("quanLyVaiTro");
            }
            return EditVaiTro(id);
        }











        //Sach
        public ActionResult quanLySach()
        {
            var all_sach = from tt in data.Saches select tt;
            return View(all_sach);
        }
       
        public ActionResult Create()
        {
            var D_theloai = from tt in data.TheLoais select tt;
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var tl in D_theloai)
                list.Add(new SelectListItem() { Value = tl.MaTL.ToString(), Text = tl.TenTL });
            ViewBag.theloai = list;
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file,Sach sach)
        {
            var D_theloai = from tt in data.TheLoais select tt;
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var tl in D_theloai)
                list.Add(new SelectListItem() { Value = tl.MaTL.ToString(), Text = tl.TenTL });
            ViewBag.theloai = list;

            string filename = Path.GetFileName(file.FileName);
           
            file.SaveAs(Server.MapPath("\\Content\\" + file.FileName));
            sach.HinhAnh = "\\Content\\" + filename;          
            data.Saches.InsertOnSubmit(sach);
         
            data.SubmitChanges();
            return RedirectToAction("quanLySach");
       
        }

        public ActionResult EditSach(int id)
        {
            var D_sach = data.Saches.Where(m => m.MaSach == id).First();
            var D_theloai = from tt in data.TheLoais select tt;
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var tl in D_theloai)
                list.Add(new SelectListItem() { Value = tl.MaTL.ToString(), Text = tl.TenTL });
            ViewBag.theloai = list;
            return View(D_sach);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSach(int id, Sach model, HttpPostedFileBase file)
        {
            var D_theloai = from tt in data.TheLoais select tt;
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var tl in D_theloai)
                list.Add(new SelectListItem() { Value = tl.MaTL.ToString(), Text = tl.TenTL });
            ViewBag.theloai = list;
            var D_sach = data.Saches.Where(m => m.MaSach == id).First();

            // Checking if any such record exist
            if (D_sach != null)
            {
                D_sach.TenSach = model.TenSach;
                D_sach.TenTacGia = model.TenTacGia;
                D_sach.MaTL = model.MaTL;
                D_sach.NhaXuatBan = model.NhaXuatBan;
                D_sach.NamXuatBan = model.NamXuatBan;
                D_sach.DonGia = model.DonGia;
                D_sach.HinhAnh = model.HinhAnh;
                D_sach.MoTa = model.MoTa;
                D_sach.SoLuongSach = model.SoLuongSach;
                string filename = Path.GetFileName(file.FileName);
                file.SaveAs(Server.MapPath("\\Content\\" + file.FileName));
                D_sach.HinhAnh = "\\Content\\" + file.FileName;
                data.SubmitChanges();
                return RedirectToAction("quanLySach");
            }
            return EditSach(id);
        }

        public ActionResult DeleteSach(int id)
        {
            var D_sach = data.Saches.First(m => m.MaSach == id);
            return View(D_sach);
        }
        [HttpPost]
        public ActionResult DeleteSach(int id, FormCollection collection)
        {
            var D_sach = data.Saches.Where(m => m.MaSach == id).First();
            data.Saches.DeleteOnSubmit(D_sach);
            data.SubmitChanges();
            return RedirectToAction("quanLySach");
        }
        
        
        
        public ActionResult themSachGiamGia(int id)
        {
            var D_sach1 = (from s in data.SachGiamGias where s.MaSach == id select s).FirstOrDefault();
            if (D_sach1 != null)
            {
                return Redirect("https://localhost:44339/Admin/quanLySach");

            }
            else
            {
                var D_sach = (from s in data.Saches where s.MaSach == id select s).First();
                var sale = new SachGiamGia();
                sale.MaSach = D_sach.MaSach;
                sale.GiaCu = D_sach.DonGia;
                sale.PhanTram = 50;
                sale.GiaMoi = D_sach.DonGia - (D_sach.DonGia * sale.PhanTram / 100);           
                data.SachGiamGias.InsertOnSubmit(sale);
                data.SubmitChanges();
                D_sach.DonGia = sale.GiaMoi;
                UpdateModel(D_sach);
                data.SubmitChanges();

            }
            
            return Redirect("https://localhost:44339/Admin/quanLySach");
        }
        //[Authorize]
        public ActionResult XoaSachGiamGia(int? id)
        {
            var D_sach = (from s in data.SachGiamGias where s.MaSach == id select s).First();
            var D_sach1 = (from s in data.Saches where s.MaSach == id select s).First();
            D_sach1.DonGia = D_sach.GiaCu;
            UpdateModel(D_sach1);
            data.SubmitChanges();
            data.SachGiamGias.DeleteOnSubmit(D_sach);
            data.SubmitChanges();
            return Redirect("https://localhost:44339/Admin/danhSachGiamGia");
        }
        public ActionResult danhSachGiamGia()
        {
            var D_sach = (from s in data.SachGiamGias select s);
            return View(D_sach.ToList());
        }
        public ActionResult EditSachGiamGia(int id)
        {
            var E_category = data.SachGiamGias.First(m => m.MaSach == id);
            return View(E_category);
        }
        [HttpPost]
        public ActionResult EditSachGiamGia(int id, FormCollection collection)
        {
            var D_sach = (from s in data.SachGiamGias where s.MaSach == id select s).First();
            var E_phantram = int.Parse(collection["PhanTram"]);        
            ////D_sach.GiaMoi = D_sach.GiaCu - (D_sach.GiaCu * (D_sach.PhanTram / 100));
            //UpdateModel(D_sach);
            //data.SubmitChanges();
            D_sach.MaSach = id;      
            D_sach.PhanTram = E_phantram;     
            D_sach.GiaMoi = D_sach.GiaCu - (D_sach.GiaCu * (E_phantram * 0.01));
            UpdateModel(D_sach);
            data.SubmitChanges();
            var sach = (from s in data.Saches where s.MaSach == D_sach.MaSach select s).First();
            sach.DonGia = D_sach.GiaMoi;
            UpdateModel(sach);
            data.SubmitChanges();
            return RedirectToAction("danhSachGiamGia");      
        }
        //don hang 
        //duyet don 1
        //chua duyet null
        //huy duyet don -1;

        //don hang chua duyet
        public ActionResult quanLyDonHang()
        {
            var all_donhang = from tt in data.HoaDons where tt.TinhTrang == null select tt;
            return View(all_donhang);
        }
        
        public ActionResult duyetDonHang(int id)
        {
            var hoadon = data.HoaDons.First(m => m.MaHD == id);
            //hoadon.TinhTrangDonHang = true;
            hoadon.TinhTrang = 1;
            UpdateModel(hoadon);
            data.SubmitChanges();
            string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send2 - Copy.html"));
            contentCustomer = contentCustomer.Replace("{{MaDon}}", hoadon.MaHD.ToString());
            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", hoadon.NguoiDung.HoTen);
            contentCustomer = contentCustomer.Replace("{{SDT}}", hoadon.NguoiDung.SDT);
            contentCustomer = contentCustomer.Replace("{{Email}}", hoadon.NguoiDung.Email);
            contentCustomer = contentCustomer.Replace("{{DiaChi}}", hoadon.NguoiDung.DiaChi);
            contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
            KK_BookStore.Models.sendEmail.SendEmail("K&K BookStore", "Đơn hàng #" + hoadon.MaHD, contentCustomer.ToString(), hoadon.NguoiDung.Email);
            return RedirectToAction("quanLyDonHang");      
        }
        public ActionResult huyDonHang(int id)
        {
            var cthoadon = from tt in data.CTHoaDons where tt.HoaDon.MaHD == id select tt;
            var hoadon = data.HoaDons.First(m => m.MaHD == id);
            string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/huyDonHang.html"));
            contentCustomer = contentCustomer.Replace("{{MaDon}}", hoadon.MaHD.ToString());
            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", hoadon.NguoiDung.HoTen);
            contentCustomer = contentCustomer.Replace("{{SDT}}", hoadon.NguoiDung.SDT);
            contentCustomer = contentCustomer.Replace("{{Email}}", hoadon.NguoiDung.Email);
            contentCustomer = contentCustomer.Replace("{{DiaChi}}", hoadon.NguoiDung.DiaChi);
            contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
            KK_BookStore.Models.sendEmail.SendEmail("K&K BookStore", "Hủy Đơn hàng #" + hoadon.MaHD, contentCustomer.ToString(), hoadon.NguoiDung.Email);
            //xoa cai dong nay
            //data.CTHoaDons.DeleteAllOnSubmit(cthoadon);
            //data.SubmitChanges();
            //data.HoaDons.DeleteOnSubmit(hoadon);
            hoadon.TinhTrang = -1;
            UpdateModel(hoadon);
            data.SubmitChanges();
            return RedirectToAction("quanLyDonHang");
        }
        public ActionResult tatCaDonHang()
        {        
            var all_donhang = from tt in data.HoaDons orderby tt.MaHD descending  select tt;
            return View(all_donhang);
        }
        public ActionResult chiTietDonHang(int id)
        {
            var all_ctdonhang = from tt in data.CTHoaDons where tt.MaHD == id select tt;
            double? tongtien = 0;
            foreach (var item in all_ctdonhang)
            {
                tongtien += item.ThanhTien;
            }

            ViewBag.Tongtien = tongtien;
            return View(all_ctdonhang);
        }

        


        [HttpPost]
        public ActionResult LayThongTin(string Id)
        {
            return ViewBag.hinh = Id;
        }
        public ActionResult danhSachPhanHoi()
        {
            var all_phanhoi = from tt in data.PhanHois select tt;
            return View(all_phanhoi);
        }


        public ActionResult danhSachThongBao(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 3;
            int pageNum = page ?? 1;
            var all_ThongBao = from tt in data.ThongBaos where tt.TaiKhoan == "Admin" orderby tt.MaThongBao descending select tt;
            return View(all_ThongBao.ToPagedList(pageNum, pageSize));
        }

        public ActionResult duyetBaiViet(int id)
        {
            var duyet_BaiViet = (from tt in data.BaiViets where tt.MaBaiViet == id select tt).First();
            var all_ThongBao = (from tt in data.ThongBaos where tt.MaBaiViet == id select tt).First();

            duyet_BaiViet.TrangThai = 1;
            UpdateModel(duyet_BaiViet);
            all_ThongBao.TrangThai = 1;
            UpdateModel(all_ThongBao);
            data.SubmitChanges();
            SetAlert("Duyệt bài thành công", "success");
            //guiTHongBaoChoReviewer
            ThongBao thongBaoReviewer = new ThongBao();
            thongBaoReviewer.NgayTao = DateTime.Now;
            thongBaoReviewer.MaBaiViet = id;
            thongBaoReviewer.TrangThai = 0;
            thongBaoReviewer.NoiDung = "Bài viêt " + id + "của bạn đã được duyệt !!";
            thongBaoReviewer.TaiKhoan = duyet_BaiViet.TaiKhoan;
            data.ThongBaos.InsertOnSubmit(thongBaoReviewer);
            data.SubmitChanges();
            LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
            lichSuHoatDong.Ngay = DateTime.Now;
            lichSuHoatDong.TaiKhoan = User.Identity.Name;
            lichSuHoatDong.MaBaiViet = duyet_BaiViet.MaBaiViet;
            lichSuHoatDong.NoiDung = "Đã duyệt bài viết";
            lichSuHoatDong.LoaiHoatDong = "duyet_Post";

            data.LichSuHoatDongs.InsertOnSubmit(lichSuHoatDong);
            return RedirectToAction("danhSachThongBao");
        }

        public ActionResult xemTruocBaiViet(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }
            var baiViet = data.BaiViets.Where(m => m.MaBaiViet == id).First();
            ViewBag.MaBaiViet = baiViet.MaBaiViet;           
            return View(baiViet);
        }
    }

}
