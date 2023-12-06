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
            //so luong thong bao chua doc
            var countNoti = mydata.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();
            var nguoidung = User.Identity.GetUserName();
            var user = mydata.NguoiDungs.Where(p => p.TaiKhoan == nguoidung);
            foreach (var item in user)
            {
               
                ViewBag.hinh = item.Hinh;
                

            }
            var tacgia = mydata.Authors;
            return View(tacgia);
        }
    }
}