using Microsoft.Security.Application;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            //lay all bai viet public
            var baiviet = myData.BaiViets.Where(m=>m.TrangThai==1);

            //lay reiviewr
            var tacgia = myData.NguoiDungs.Where(m => m.MaChucVu == 1004);
            ViewBag.lstTacGia = tacgia;

            //layBaiVietMoi
            var baivietmoi = from s in myData.BaiViets where s.TrangThai==1 orderby s.NgayViet descending select s;
            List<BaiViet> baiVietMoi = new List<BaiViet>();
            foreach(var item in baivietmoi)
            {
                baiVietMoi.Add(item);
                if (baiVietMoi.Count == 6)
                    break;
            }
            ViewBag.baiVietMoi = baiVietMoi;

            //lay post nhieu danhgiacao
            var baiVietDanhGiaCao = from tt in myData.BaiViets orderby tt.SoSao descending select tt;
            List<BaiViet> lstbaiVietDanhGiaCao = new List<BaiViet>();
            foreach (var item in baiVietDanhGiaCao)
            {
                lstbaiVietDanhGiaCao.Add(item);
                if (lstbaiVietDanhGiaCao.Count() == 4)
                    break;
            }
            ViewBag.baiVietDanhGiaCao = lstbaiVietDanhGiaCao;
            ////layRanDom
            //var random = myData.BaiViets.OrderBy(o => Guid.NewGuid());
            //List<BaiViet> randomData = new List<BaiViet>();
            //foreach (var item in random)
            //{
            //    randomData.Add(item);
            //    if (randomData.Count == 6)
            //        break;
            //}
            //ViewBag.randomData = randomData;

            //baiVietNhieuTim
            var baiVietNhieuTim = from tt in myData.BaiViets orderby tt.YeuThich descending select tt;
            List<BaiViet> lstbaiVietNhieuTim = new List<BaiViet>();
            foreach (var item in baiVietNhieuTim)
            {
                lstbaiVietNhieuTim.Add(item);
                if (lstbaiVietNhieuTim.Count == 6)
                    break;
            }
            ViewBag.lstbaiVietNhieuTim = lstbaiVietNhieuTim;
            return View(baiviet);
        }

        //BaiViet
        [Authorize(Roles ="Reviewer")]
        public ActionResult quanLyBaiViet()
        {
            var nguoidung = myData.NguoiDungs.Where(m => m.TaiKhoan == User.Identity.Name);
            ViewBag.hinh = nguoidung.First().Hinh;
            var allPost = myData.BaiViets.Where(m =>m.TaiKhoan == User.Identity.Name);                               
            return View(allPost);
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
        [Authorize(Roles = "Reviewer")]
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in myData.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }
            //list the loai
            var alltheloai = myData.TheLoais.ToList();
            ViewBag.TheLoais = alltheloai;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in myData.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }
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
            baiViet.SoSao = 0;
            baiViet.SoLuotDanhGia = 0;
            baiViet.YeuThich = 0;
            baiViet.LuotXem = 0;
            myData.BaiViets.InsertOnSubmit(baiViet);
            myData.SubmitChanges();
            return Redirect("https://localhost:44339/Review/quanLyBaiViet/"); ;
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

            var cmt = myData.BinhLuans.Where(p => p.MaBaiViet == id);
            if (cmt != null)
            {
                ViewBag.slCMT = cmt.Count();
                ViewBag.cmt = cmt;
            }
               
            else
                ViewBag.checkCMT = -1;
            
            var baiViet = myData.BaiViets.Where(m => m.MaBaiViet == id).First();
            ViewBag.MaBaiViet = baiViet.MaBaiViet;
            baiViet.LuotXem++;
            UpdateModel(baiViet);
            myData.SubmitChanges();
            return View(baiViet);
        }
        [Authorize]
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
        [Authorize(Roles = "Reviewer")]
        public ActionResult Edit(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in myData.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }
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
            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in myData.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }
            var alltheloai = myData.TheLoais.ToList();
            ViewBag.TheLoais = alltheloai;
            var baiViet = myData.BaiViets.Where(m => m.MaBaiViet == id).First();
            if (baiViet != null)
            {
                baiViet.TenBaiViet = model.TenBaiViet;
                baiViet.AnhBia = model.AnhBia;
                baiViet.NoiDung = model.NoiDung;
                baiViet.MaTL = model.MaTL;
                baiViet.MoTa = model.MoTa;
                baiViet.TrangThai = model.TrangThai;             
                myData.SubmitChanges();
            }
            if (baiViet.TrangThai == 0)
                ViewBag.status = 0;
            if (baiViet.TrangThai == 1)
                ViewBag.status = 1;
            ViewBag.TL = baiViet.MaTL;
            return Redirect("https://localhost:44339/Review/quanLyBaiViet");
        }



        [Authorize]
        public ActionResult themBinhLuan(int mabaiviet, string comment, int? rate)
        {

            

            var D_tk = myData.CTBinhLuans.FirstOrDefault(m => m.BinhLuan.TaiKhoan.Equals(User.Identity.Name) && m.BinhLuan.MaBaiViet == mabaiviet);
            var lst_DanhGia = myData.BinhLuans.Where(m => m.MaBaiViet == mabaiviet);
            var D_sach = myData.BaiViets.Where(m => m.MaBaiViet == mabaiviet).First();
            if (D_tk == null)
            {
                BinhLuan danhGia = new BinhLuan();
                danhGia.MaBaiViet = mabaiviet;
                danhGia.NgayTao = DateTime.Now;
                danhGia.TaiKhoan = User.Identity.Name;
                danhGia.SoLike = 0;
                //var soSao = collection["SoSao"];
                var soSao = rate;
                danhGia.SoSao = soSao;
                danhGia.NoiDung = comment;
                myData.BinhLuans.InsertOnSubmit(danhGia);
                myData.SubmitChanges();
                CTBinhLuan cTDanhGia = new CTBinhLuan();
                cTDanhGia.MaBinhLuan = danhGia.MaBinhLuan;
                int? sl = 0;
                foreach (var item in lst_DanhGia)
                {
                    sl += item.SoSao;
                }
                cTDanhGia.TongSao = sl;
                cTDanhGia.DiemDanhGia = (float)cTDanhGia.TongSao / lst_DanhGia.Count();
                myData.CTBinhLuans.InsertOnSubmit(cTDanhGia);
                D_sach.SoSao =(float) cTDanhGia.TongSao / lst_DanhGia.Count();
                D_sach.SoLuotDanhGia++;
                UpdateModel(D_sach);
                myData.SubmitChanges();
            }
            else
            {
                D_tk.BinhLuan.SoSao = rate;
                D_tk.BinhLuan.NoiDung = comment;
                D_tk.BinhLuan.NgayTao = DateTime.Now;
                int? sl = 0;
                foreach (var item in lst_DanhGia)
                {
                    sl += item.SoSao;
                }
                D_tk.TongSao = sl;
                UpdateModel(D_tk);
                var tinhSao = myData.CTBinhLuans.Where(m => m.BinhLuan.MaBaiViet == mabaiviet).AsEnumerable().Last();
                tinhSao.TongSao = sl;
                tinhSao.DiemDanhGia = (float)sl / lst_DanhGia.Count();

                UpdateModel(tinhSao);
                D_sach.SoSao = (float)sl / lst_DanhGia.Count();
                UpdateModel(D_sach);
                myData.SubmitChanges();
            }

            myData.SubmitChanges();
            return Redirect("https://localhost:44339/Review/Detail/" + mabaiviet);
        }
        
        public ActionResult danhSachBaiViet(int? page)
        {

            if (page == null) page = 1;
            int pageSize = 6;
            int pageNum = page ?? 1;
            var allPost = from tt in myData.BaiViets select tt;


            var nguoidung = from tt in myData.NguoiDungs where tt.TaiKhoan == User.Identity.Name select tt;
            ViewBag.hinh = nguoidung.First().Hinh;
            //lay the loai
            var allCate = from tt in myData.TheLoais select tt;
            ViewBag.TheLoai = allCate;
            //lay post nhieu tim/view
            var baiVietNhieuTim = from tt in myData.BaiViets orderby tt.SoSao descending select tt;
            List<BaiViet> lstBaiVietNhieuTim = new List<BaiViet>();
            foreach(var item in baiVietNhieuTim)
            {
                lstBaiVietNhieuTim.Add(item);
                if (lstBaiVietNhieuTim.Count() == 3)
                    break;
            }
            ViewBag.baiVietNhieuTim = lstBaiVietNhieuTim;


            return View(allPost.ToPagedList(pageNum, pageSize));
        }
        public ActionResult locTheLoai(int id, int? page )
        {
            if (page == null) page = 1;
            int pageSize = 3;
            int pageNum = page ?? 1;
            var allPost = from tt in myData.BaiViets select tt;



            ViewBag.hinh = allPost.First().NguoiDung.Hinh;
            //lay the loai
            var allCate = from tt in myData.TheLoais select tt;
            ViewBag.TheLoai = allCate;
            //lay post nhieu tim/view
            var baiVietNhieuTim = from tt in myData.BaiViets orderby tt.SoSao descending select tt;
            List<BaiViet> lstBaiVietNhieuTim = new List<BaiViet>();
            foreach (var item in baiVietNhieuTim)
            {
                lstBaiVietNhieuTim.Add(item);
                if (lstBaiVietNhieuTim.Count() == 3)
                    break;
            }
            ViewBag.baiVietNhieuTim = lstBaiVietNhieuTim;


            var cate = from tt in myData.BaiViets where tt.MaTL == id select tt;
            return View(cate.ToPagedList(pageNum, pageSize));
        }

    }
}