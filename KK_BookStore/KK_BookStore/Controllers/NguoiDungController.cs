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
        private MyDataDataContext mydata = new MyDataDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(string id)
        {
            var E_sach = mydata.NguoiDungs.First(m => m.TaiKhoan.Equals(id));
            return View(E_sach);
        }
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            var E_taikhoan = mydata.NguoiDungs.First(m => m.TaiKhoan.Equals(id));
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
                mydata.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }
        public ActionResult trangCaNhan()
        {
            var nguoidung = User.Identity.GetUserName();
            var user = mydata.NguoiDungs.Where(p => p.TaiKhoan == nguoidung);
            foreach (var item in user)
            {

                ViewBag.hinh = item.Hinh;


            }
            return View();
        }
    }
}