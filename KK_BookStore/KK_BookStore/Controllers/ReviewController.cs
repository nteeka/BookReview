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
            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in myData.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }

            var checkYeuThich = myData.DanhSachYeuThiches.Where(m=>m.TaiKhoan==User.Identity.Name).ToList().FirstOrDefault();
            if (checkYeuThich!=null)
                ViewBag.check = checkYeuThich;
            var baiviet = myData.BaiViets.Where(m=>m.TrangThai==1);                                           
            return View(baiviet);
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
            //list the loai
            var alltheloai = myData.TheLoais.ToList();
            ViewBag.TheLoais = alltheloai;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            var alltheloai = myData.TheLoais.ToList();
            ViewBag.TheLoais = alltheloai;
            //create bai viet
            BaiViet baiViet = new BaiViet();
            baiViet.TaiKhoan = User.Identity.Name;
            baiViet.NgayViet = DateTime.Now;
            baiViet.TenBaiViet = collection["TenBaiViet"];
            //string xoaHtml = Sanitizer.GetSafeHtmlFragment(collection["NoiDung"]);
            baiViet.NoiDung = collection["NoiDung"];
            baiViet.AnhBia = collection["AnhBia"];
            baiViet.MaTL = int.Parse(collection["MaTL"]);
            baiViet.MoTa = collection["MoTa"];
            baiViet.TrangThai = int.Parse(collection["TrangThai"]);
            myData.BaiViets.InsertOnSubmit(baiViet);
            myData.SubmitChanges();
            return View();
        }



        //public ActionResult luuNhap(string TenBaiViet,String NoiDung,String AnhBia,int MaTL, String MoTa)
        //{
        //    BaiViet baiViet = new BaiViet();
        //    baiViet.TaiKhoan = User.Identity.Name;
        //    baiViet.NgayViet = DateTime.Now;
        //    baiViet.TenBaiViet = TenBaiViet;
        //    baiViet.NoiDung =NoiDung;
        //    baiViet.AnhBia = AnhBia;
        //    baiViet.MaTL = MaTL;
        //    baiViet.MoTa = MoTa;
        //    myData.BaiViets.InsertOnSubmit(baiViet);
        //    myData.SubmitChanges();

        //    return Redirect("https://localhost:44339/Sach/danhSachBinhLuan/");
        //}
        public ActionResult dangBai()
        {


            return Redirect("https://localhost:44339/Sach/danhSachBinhLuan/");
        }
        public ActionResult Detail(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in myData.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }

            var cmt = myData.BinhLuans.Where(p => p.MaSach == 1);
            ViewBag.cmt = cmt;
            var baiViet = myData.BaiViets.Where(m => m.MaBaiViet == id).First();
            baiViet.LuotXem++;
            UpdateModel(baiViet);
            myData.SubmitChanges();
            return View(baiViet);
        }

        public ActionResult yeuThichBaiViet(int id, string strURL)
        {
            
            var baiviet = (from s in myData.BaiViets where s.MaBaiViet == id select s).First();
            int? temp = baiviet.MaBaiViet;
            var check = myData.DanhSachYeuThiches.Where(m => m.MaBaiViet == id && m.TaiKhoan.Equals(User.Identity.Name)).FirstOrDefault();
            //th chua like
            if (check == null)
            {
                DanhSachYeuThich danhSachYeuThich = new DanhSachYeuThich();
                danhSachYeuThich.MaBaiViet = (int)id;
                danhSachYeuThich.TaiKhoan = User.Identity.Name;
                UpdateModel(danhSachYeuThich);
                myData.DanhSachYeuThiches.InsertOnSubmit(danhSachYeuThich);
                baiviet.YeuThich++;
                UpdateModel(baiviet);
                myData.SubmitChanges();
            }
            else
            {
                var delete = (from s in myData.DanhSachYeuThiches where s.MaBaiViet == id && s.TaiKhoan.Equals(User.Identity.Name) select s).First();
                myData.DanhSachYeuThiches.DeleteOnSubmit(delete);
                baiviet.YeuThich--;
                UpdateModel(baiviet);
                myData.SubmitChanges();
            }
            return Redirect(strURL);
        }
        public ActionResult Edit(int id)
        {
            var alltheloai = myData.TheLoais.ToList();
            ViewBag.TheLoais = alltheloai;
            var baiViet = myData.BaiViets.Where(m => m.MaBaiViet == id).First();
            ViewBag.TL = baiViet.MaTL;
            if (baiViet.TrangThai == 0)
                ViewBag.status = 0;
            if (baiViet.TrangThai == 1)
                ViewBag.status = 1;
            return View(baiViet);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, BaiViet model)
        {
            var alltheloai = myData.TheLoais.ToList();
            ViewBag.TheLoais = alltheloai;
            var baiViet = myData.BaiViets.Where(m => m.MaBaiViet == id).First();
            if (baiViet != null)
            {
                baiViet.TenBaiViet = model.TenBaiViet;
                baiViet.AnhBia = model.AnhBia;
                baiViet.NoiDung = model.NoiDung;
                //baiViet.MaTL = model.MaTL;
                baiViet.MoTa = model.MoTa;
                baiViet.TrangThai = model.TrangThai;             
                myData.SubmitChanges();
            }
            if (baiViet.TrangThai == 0)
                ViewBag.status = 0;
            if (baiViet.TrangThai == 1)
                ViewBag.status = 1;
            ViewBag.TL = baiViet.MaTL;
            return View(baiViet);
        }

    }
}