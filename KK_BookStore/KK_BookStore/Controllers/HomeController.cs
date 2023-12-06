using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace KK_BookStore.Controllers
{
    public class HomeController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        public ActionResult Index()
        {
            //so luong truy cap he thong
            SystemAccess systemAccess = new SystemAccess();
            systemAccess.Ngay = DateTime.Now;
            systemAccess.SoLuongTruyCap=1;
            
            data.SystemAccesses.InsertOnSubmit(systemAccess);
            data.SubmitChanges();
            //var demTC = data.SystemAccesses.Sum(m => m.SoLuongTruyCap);
            var all_sgg = (from s in data.SachGiamGias select s);
            ViewBag.sgg = all_sgg;
            ViewBag.sl = 0;
            //var all_sach = (from s in data.SachGiamGias  select s);
            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }
            
            var baiviets = data.BaiViets.OrderByDescending(m=>m.SoSao).ToList();
            List<BaiViet> lstBaiViet = new List<BaiViet>();
            int count = 0;
            foreach(var item in baiviets)
            {
                lstBaiViet.Add(item);
                count++;
                if (count == 3)
                    break;
            }
            ViewBag.baiviet = lstBaiViet;


            List<BaiViet> baivietsRanDom = data.BaiViets.Where(m=>m.TrangThai==1).ToList();
            List<BaiViet> random = new List<BaiViet>();
            Random rand = new Random();
            for (int i = 0;i<3;i++)
            {
                int k = rand.Next(0, baivietsRanDom.Count);             
                
                    random.Add(baivietsRanDom[k]);
            }
            ViewBag.random = random;

            //dem post
            ViewBag.countPost = baivietsRanDom.Count();
            //dem cmt
            var countComment = data.BinhLuans.ToList();
            ViewBag.countComments = countComment.Count();
            //dem visitor
            var countVisitor = data.SystemAccesses.ToList();
            ViewBag.countVisitors = countVisitor.Count();

            //so luong thong bao chua doc
            var countNoti = data.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();

            return View();
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            //so luong thong bao chua doc
            var countNoti = data.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        



    }
}