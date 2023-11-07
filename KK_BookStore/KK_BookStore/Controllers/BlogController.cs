using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace KK_BookStore.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        private MyDataDataContext mydata = new MyDataDataContext();
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserName();
           
            var user = mydata.NguoiDungs.Where(p => p.TaiKhoan == userId);
            foreach (var item in user)
            {
                //ViewBag.Email = item.Email;
                //ViewBag.Fullname = item.HoTen;
                //ViewBag.Date = item.NgaySinh;
                //ViewBag.Address = item.DiaChi;
                ViewBag.hinh = item.Hinh;
                //ViewBag.PhoneNumber = item.SDT;
            }
            return View();
        }
    }
}