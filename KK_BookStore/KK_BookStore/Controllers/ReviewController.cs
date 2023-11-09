using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KK_BookStore.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review
        MyDataDataContext myData = new MyDataDataContext();
        public ActionResult Index()
        {
            var baiviet = myData.BaiViets.Where(p => p.MaBaiViet == 1).First();
            ViewBag.baiviet = baiviet;
            var chitietbaiviet = myData.ChiTietBaiViets.Where(p => p.MaBaiViet == 1);
            ViewBag.chitietbaiviet = chitietbaiviet;
            var cmt = myData.BinhLuans.Where(p => p.MaSach == 1);
            ViewBag.cmt = cmt;
            
            return View();
        }
    }
}