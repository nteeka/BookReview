using KK_BookStore.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult themBinhLuan(int mabaiviet, string comment)
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
            return Redirect("https://localhost:44339/Review/Detail/" + mabaiviet);
        }
        [Authorize]
        public ActionResult themPhanHoi(int mabinhluan, string comment)
        {
            var cmt = data.BinhLuans.Where(m => m.MaBinhLuan == mabinhluan).First();
            PhanHoi phanHoi = new PhanHoi();
            phanHoi.MaBinhLuan = mabinhluan;
            phanHoi.NgayTao = DateTime.Now;
            phanHoi.TaiKhoan = User.Identity.Name;
            phanHoi.NoiDung = "@" + cmt.TaiKhoan + comment;
            data.PhanHois.InsertOnSubmit(phanHoi);
            data.SubmitChanges();
            return Redirect("https://localhost:44339/Review/danhSachPhanHoi/" + phanHoi.MaBinhLuan);
        }

        [Authorize]
        public ActionResult thichBinhLuan(int id)
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
            }
            else
            {
                var delete = (from s in data.ChiTietBinhLuans where s.MaBinhLuan == id && s.TaiKhoan.Equals(User.Identity.Name) select s).First();
                data.ChiTietBinhLuans.DeleteOnSubmit(delete);
                binhLuan.SoLike--;
                UpdateModel(binhLuan);
                data.SubmitChanges();
                
            }
            return Redirect("https://localhost:44339/Review/Detail/" + binhLuan.MaBaiViet);
        }



        public ActionResult trangCaNhan()
        {
            var nguoidung = User.Identity.GetUserName();
            var user = data.NguoiDungs.Where(p => p.TaiKhoan == nguoidung);
            foreach (var item in user)
            {

                ViewBag.hinh = item.Hinh;


            }
            return View();
        }
    }
}