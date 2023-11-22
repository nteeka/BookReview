using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace KK_BookStore.Controllers
{
    public class TacGiaController : Controller
    {
        // GET: TacGia
        private MyDataDataContext mydata = new MyDataDataContext();
        public ActionResult Index()
        {
            var nguoidung = User.Identity.GetUserName();
            var user = mydata.NguoiDungs.Where(p => p.TaiKhoan == nguoidung);
            foreach (var item in user)
            {
               
                ViewBag.hinh = item.Hinh;
                

            }
            var tacgia = mydata.NguoiDungs.Where(p => p.MaChucVu == 1004);
            return View(tacgia);
        }
    }
}