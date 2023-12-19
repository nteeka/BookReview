//using Microsoft.Ajax.Utilities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace KK_BookStore.Controllers
//{
//    public class TheLoaiController : Controller
//    {
//        // GET: TheLoai
//        MyDataDataContext myData = new MyDataDataContext();
//        public ActionResult Index()
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                var anhDaiDien = from s in myData.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
//                ViewBag.hinh = anhDaiDien.First().Hinh;
//            }
//            var all = from tl in myData.TheLoais select tl;
//            var all_sach = from tl in myData.Saches select tl;
//            ViewBag.sach = all_sach;
//            var sachGiamGia = from s in myData.SachGiamGias select s;
//            ViewBag.sachGiamGia = sachGiamGia;
//            //var all_theloai = from tl in myData.Saches select tl;
//            return View(all.ToList());    
//        }
//        public ViewResult SachTheoTheLoai(int MaTL = 0)
//        {
//            var sachGiamGia = from s in myData.SachGiamGias select s;
//            ViewBag.sachGiamGia = sachGiamGia;
//            if (User.Identity.IsAuthenticated)
//            {
//                var anhDaiDien = from s in myData.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
//                ViewBag.hinh = anhDaiDien.First().Hinh;
//            }
//            //Kiểm tra chủ đề tồn tại hay hông
//            TheLoai tl =myData.TheLoais.SingleOrDefault(n => n.MaTL == MaTL);
//            if (tl == null)
//            {
//                Response.StatusCode = 404;
//                return null;
//            }
//            //Truy suất danh sách các quyển sách theo chủ đề
//            List<Sach> lstSach = myData.Saches.Where(n => n.MaTL == MaTL).OrderBy(n => n.DonGia).ToList();
//            if (lstSach.Count == 0)
//            {
//                ViewBag.Sach = "Không có sách nào thuộc thể loại này";
//            }
//            ViewBag.ten = tl.TenTL;
//            return View(lstSach);

//        }
//    }
//}