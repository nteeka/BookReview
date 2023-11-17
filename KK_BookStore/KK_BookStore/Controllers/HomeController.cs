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
            var all_sgg = (from s in data.SachGiamGias select s);
            ViewBag.sgg = all_sgg;
            ViewBag.sl = 0;
            //var all_sach = (from s in data.SachGiamGias  select s);
            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }
            
            var baiviets = data.BaiViets.ToList();
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
            
            //return View(all_sach.ToList());
            return View();
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        



    }
}