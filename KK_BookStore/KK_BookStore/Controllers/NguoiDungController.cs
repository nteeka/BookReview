using KK_BookStore.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace KK_BookStore.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        private ApplicationDbContext db = new ApplicationDbContext();
        private MyDataDataContext data = new MyDataDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(string id)
        {
            var E_sach = data.NguoiDungs.First(m => m.TaiKhoan.Equals(id));
            return View(E_sach);
        }
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            var E_taikhoan = data.NguoiDungs.First(m => m.TaiKhoan.Equals(id));
            var E_hoten = collection["HoTen"];
            var E_ngaysinh = (collection["NgaySinh"]);
            var E_diachi = collection["DiaChi"];
            var E_email = collection["Email"];
            E_taikhoan.TaiKhoan = id;
            if (string.IsNullOrEmpty(E_hoten))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_taikhoan.HoTen = E_hoten;
                E_taikhoan.NgaySinh = E_ngaysinh;
                E_taikhoan.DiaChi = E_diachi;
                E_taikhoan.Email = E_email;
                UpdateModel(E_taikhoan);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }

        [Authorize]
        public ActionResult themBinhLuan(int mabaiviet, string comment,string strURL)
        {    
            //var lst_DanhGia = data.BinhLuans.Where(m => m.MaBaiViet == mabaiviet);
             BinhLuan binhLuan = new BinhLuan();
            binhLuan.MaBaiViet = mabaiviet;
            binhLuan.NgayTao = DateTime.Now;
            binhLuan.TaiKhoan = User.Identity.Name;
            binhLuan.NoiDung = comment;
            binhLuan.SoLike = 0;
            data.BinhLuans.InsertOnSubmit(binhLuan);
            data.SubmitChanges();
            LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
            lichSuHoatDong.Ngay = DateTime.Now;
            lichSuHoatDong.TaiKhoan = User.Identity.Name;
            lichSuHoatDong.MaBinhLuan = binhLuan.MaBinhLuan;
            lichSuHoatDong.MaBaiViet = binhLuan.MaBaiViet;
            lichSuHoatDong.NoiDung ="Đã bình luận bài viết";
            lichSuHoatDong.LoaiHoatDong = "cmt";
            
            data.LichSuHoatDongs.InsertOnSubmit(lichSuHoatDong);
            data.SubmitChanges();                                     
            return Redirect(strURL);
        }
        [Authorize]
        public ActionResult themPhanHoi(int mabinhluan, string comment, string strURL)
        {
           
            var cmt = data.BinhLuans.Where(m => m.MaBinhLuan == mabinhluan).First();

            PhanHoi phanHoi = new PhanHoi();
            phanHoi.MaBinhLuan = mabinhluan;
            phanHoi.NgayTao = DateTime.Now;
            phanHoi.TaiKhoan = User.Identity.Name;
            phanHoi.NoiDung = "@" + cmt.TaiKhoan + comment;
            data.PhanHois.InsertOnSubmit(phanHoi);
            data.SubmitChanges();
            LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
            lichSuHoatDong.Ngay = DateTime.Now;
            lichSuHoatDong.TaiKhoan = User.Identity.Name;
            lichSuHoatDong.MaPhanHoi = phanHoi.MaPhanHoi;
            lichSuHoatDong.MaBaiViet = phanHoi.BinhLuan.MaBaiViet;
            lichSuHoatDong.NoiDung = "Đã phản hồi một bình luận";
            lichSuHoatDong.LoaiHoatDong = "reply";

            data.LichSuHoatDongs.InsertOnSubmit(lichSuHoatDong);
            data.SubmitChanges();
            return Redirect(strURL);
        }
        
        [Authorize]
        public ActionResult thichBinhLuan(int id, string strURL)
        {
            var binhLuan = (from s in data.BinhLuans where s.MaBinhLuan == id select s).First();        
            var check = data.ChiTietBinhLuans.Where(m => m.MaBinhLuan == id && m.TaiKhoan.Equals(User.Identity.Name)).FirstOrDefault();
            //th chua like
            if (check == null)
            {
                ChiTietBinhLuan cTLike = new ChiTietBinhLuan();
                cTLike.MaBinhLuan = (int)id;
                cTLike.TaiKhoan = User.Identity.Name;
                UpdateModel(cTLike);
                data.ChiTietBinhLuans.InsertOnSubmit(cTLike);
                binhLuan.SoLike++;
                UpdateModel(binhLuan);
                data.SubmitChanges();
                LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                lichSuHoatDong.Ngay = DateTime.Now;
                lichSuHoatDong.TaiKhoan = User.Identity.Name;
                lichSuHoatDong.MaBinhLuan = binhLuan.MaBinhLuan;
                lichSuHoatDong.MaBaiViet = binhLuan.MaBaiViet;
                lichSuHoatDong.NoiDung = "Đã thích một bình luận";
                lichSuHoatDong.LoaiHoatDong = "like_cmt";

                data.LichSuHoatDongs.InsertOnSubmit(lichSuHoatDong);
                data.SubmitChanges();
            }
            else
            {
                var delete = (from s in data.ChiTietBinhLuans where s.MaBinhLuan == id && s.TaiKhoan.Equals(User.Identity.Name) select s).First();
                data.ChiTietBinhLuans.DeleteOnSubmit(delete);
                binhLuan.SoLike--;
                UpdateModel(binhLuan);
                data.SubmitChanges();
                
            }
            return Redirect(strURL); ;
        }

        [Authorize]
        public ActionResult folowAuthor(int id,string strURL)
        {

            var author = data.Authors.Where(m => m.Id_Author == id).First();            
            FollowDetail fd = new FollowDetail();
            fd.Id_Author = author.Id_Author;
            fd.TaiKhoan = User.Identity.Name;
            data.FollowDetails.InsertOnSubmit(fd);
            author.SoLuongTheoDoi++;
            UpdateModel(author);

            //thong bao chon reviewer co nguoi folow
            ThongBao thongBao = new ThongBao();
            thongBao.TaiKhoan = author.TaiKhoan;
            thongBao.TrangThai = 0;
            thongBao.NgayTao = DateTime.Now;
            thongBao.NoiDung = "@" + User.Identity.Name + " đã theo dõi bạn!";
            thongBao.MaBaiViet = 3002;
            data.ThongBaos.InsertOnSubmit(thongBao);

            //lich su hoat dong
            LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
            lichSuHoatDong.Ngay = DateTime.Now;
            lichSuHoatDong.NoiDung = "Bạn đã bắt đầu theo dõi reviewer " + author.TaiKhoan;
            lichSuHoatDong.TaiKhoan = User.Identity.Name;
            lichSuHoatDong.LoaiHoatDong = "Folow";
            data.LichSuHoatDongs.InsertOnSubmit(lichSuHoatDong);
            data.SubmitChanges();
            return Redirect(strURL); ;
        }
        [Authorize]
        public ActionResult unFolowAuthor(int id, string strURL)
        {
            var author = data.Authors.Where(m => m.Id_Author == id).First();
            var followDT = data.FollowDetails.Where(m => m.TaiKhoan == User.Identity.Name && m.Id_Author == id).First();          
            data.FollowDetails.DeleteOnSubmit(followDT);
            author.SoLuongTheoDoi--;
            UpdateModel(author);
            //lich su hoat dong
            LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
            lichSuHoatDong.Ngay = DateTime.Now;
            lichSuHoatDong.NoiDung = "Bạn đã hủy theo dõi reviewer " + author.TaiKhoan;
            lichSuHoatDong.TaiKhoan = User.Identity.Name;
            lichSuHoatDong.LoaiHoatDong = "unFollow";
            data.LichSuHoatDongs.InsertOnSubmit(lichSuHoatDong);
            data.SubmitChanges();
            return Redirect(strURL); ;
        }


        [Authorize]
        public ActionResult trangCaNhan(string name)
        {         

            var anhDaiDien = data.NguoiDungs.Where(p => p.TaiKhoan == User.Identity.Name).First();
            var user = data.Authors.Where(p => p.TaiKhoan == name).First();
            var post = data.BaiViets.Where(p => p.TaiKhoan == name && p.TrangThai == 1);
            ViewBag.posts = post;
            ViewBag.tongLike = post.Sum(m => m.YeuThich);
            ViewBag.tongPost = post.Count();
            ViewBag.tongFollow = user.SoLuongTheoDoi;
            ViewBag.trungBinhRatting = post.Sum(m => m.SoSao)/ post.Count();
            var cmt = data.BinhLuans.ToList();
            ViewBag.cmts = cmt;
            ViewBag.hinh = anhDaiDien.Hinh;
            //count CMT
            ViewBag.countComment = 0;
            ViewBag.checkCMT = 0;
            Dictionary<int, int> categoryCounts = new Dictionary<int, int>();

            foreach (var product in cmt)
            {

                // Nếu loại sản phẩm đã tồn tại trong dictionary, tăng số lượng lên 1
                if (categoryCounts.ContainsKey(product.MaBaiViet))
                {
                    categoryCounts[product.MaBaiViet]++;
                }
                // Ngược lại, thêm loại sản phẩm mới vào dictionary với số lượng là 1
                else
                {
                    categoryCounts.Add(product.MaBaiViet, 1);
                }
            }

            ViewBag.CategoryCounts = categoryCounts;
            //check yeu thich bai viet
            var check_YeuThich = data.DanhSachYeuThiches.Where(m=>m.TaiKhoan == User.Identity.Name);
            ViewBag.checkYT = check_YeuThich;
            ViewBag.tempYT = 0;

            //check folows
            var checkFollow = data.FollowDetails.Where(m => m.TaiKhoan == User.Identity.Name && m.Author.TaiKhoan == name).FirstOrDefault();
            if(checkFollow == null)
                ViewBag.checkFollow = 0;
            else
                ViewBag.checkFollow = 1;
            //so luong thong bao chua doc
            var countNoti = data.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();
            return View(user);
        }


        //view THong Bao
        public ActionResult danhSachThongBao(string name, int? page)
        {
            //so luong thong bao chua doc
            var countNoti = data.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();
            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }
            if (page == null) page = 1;
            int pageSize = 3;
            int pageNum = page ?? 1;
            var all_ThongBao = from tt in data.ThongBaos where tt.TaiKhoan == name orderby tt.MaThongBao descending select tt;
            return View(all_ThongBao.ToPagedList(pageNum, pageSize));
        }
    }
}