using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.IO;
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
            var baiviet = myData.BaiViets.Where(p => p.MaBaiViet == 2031).First();
            ViewBag.baiviet = baiviet;
            var chitietbaiviet = myData.ChiTietBaiViets.Where(p => p.MaBaiViet == 1);
            ViewBag.chitietbaiviet = chitietbaiviet;
            var cmt = myData.BinhLuans.Where(p => p.MaSach == 1);
            ViewBag.cmt = cmt;
            
            return View();
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/Img_Profile/" + file.FileName));
            ViewBag.hinh = "/Content/Img_Profile/" + file.FileName;
            return "/Content/Img_Profile/" + file.FileName;
        }
        public ActionResult Create()
        {     
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            BaiViet baiViet = new BaiViet();
            baiViet.TaiKhoan = User.Identity.Name;
            baiViet.NgayViet = DateTime.Now;
            baiViet.TenBaiViet = collection["TenBaiViet"];
            //string xoaHtml = Sanitizer.GetSafeHtmlFragment(collection["NoiDung"]);
            baiViet.NoiDung = collection["NoiDung"];
            baiViet.AnhBia = collection["AnhBia"];
            myData.BaiViets.InsertOnSubmit(baiViet);
            myData.SubmitChanges();
            return View();
        }
    }
}