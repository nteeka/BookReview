﻿using KK_BookStore.Models;
using Microsoft.Security.Application;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace KK_BookStore.Controllers
{
    public class ReviewController : BaseController
    {
        // GET: Review
        MyDataDataContext myData = new MyDataDataContext();
        public ActionResult Index()
        {
            //so luong thong bao chua doc
            var countNoti = myData.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();

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
            var tacgia = myData.NguoiDungs.Where(m => m.MaChucVu == "f72cd8b7-6945-4fba-9d72-28b9e6240acf");
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
                if (lstbaiVietDanhGiaCao.Count() == 6)
                    break;
            }
            ViewBag.baiVietDanhGiaCao = lstbaiVietDanhGiaCao;
            //layRanDom
            List<BaiViet> baivietsRanDom = myData.BaiViets.Where(m=>m.TrangThai==1).ToList();
            List<BaiViet> random = new List<BaiViet>();
            Random rand = new Random();
            for (int i = 0; i < 6; i++)
            {
                int k = rand.Next(0, baivietsRanDom.Count);

                random.Add(baivietsRanDom[k]);
            }
            // Perform Fisher-Yates Shuffle
            ViewBag.random = random;

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
        //[Authorize(Roles = "Admin")]
        //[Authorize(Roles ="Reviewer")]
        [Authorize(Roles = "Admin, Reviewer")]
        public ActionResult quanLyBaiViet(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 5;
            int pageNum = page ?? 1;
            var nguoidung = myData.NguoiDungs.Where(m => m.TaiKhoan == User.Identity.Name);
            ViewBag.hinh = nguoidung.First().Hinh;
            var allPost = myData.BaiViets.Where(m =>m.TaiKhoan == User.Identity.Name).OrderByDescending(m=>m.MaBaiViet);                               
            return View(allPost.ToPagedList(pageNum, pageSize));
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
            baiViet.TrangThai = -1;
            baiViet.SoSao = 0;
            baiViet.SoLuotDanhGia = 0;
            baiViet.YeuThich = 0;
            baiViet.LuotXem = 0;
            myData.BaiViets.InsertOnSubmit(baiViet);
            myData.SubmitChanges();
            //thong bao len man hinh
            SetAlert("Create new post success, please wait to admin check and public !!","success");
            //thong bao cho nguoi duyet
            //
            ThongBao thongBao = new ThongBao();
            thongBao.NgayTao = DateTime.Now;
            thongBao.MaBaiViet = baiViet.MaBaiViet;
            thongBao.TrangThai = 0;//chua xu ly
            thongBao.NoiDung = "Bài viết" + " đang đợi phê duyệt!!";
            thongBao.TaiKhoan = "Admin";
            myData.ThongBaos.InsertOnSubmit(thongBao);
            myData.SubmitChanges();

            


            //luu vao lich su hoat dong
            LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
            lichSuHoatDong.Ngay = DateTime.Now;
            lichSuHoatDong.TaiKhoan = User.Identity.Name;
            lichSuHoatDong.MaBaiViet = baiViet.MaBaiViet;
            lichSuHoatDong.NoiDung = "Đã viết một bài review";
            lichSuHoatDong.LoaiHoatDong = "write_post";

            myData.LichSuHoatDongs.InsertOnSubmit(lichSuHoatDong);
            myData.SubmitChanges();
            //return Redirect("https://localhost:44339/Review/quanLyBaiViet/");

            return RedirectToAction("quanLyBaiViet"); ;
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
            //so luong thong bao chua doc
            var countNoti = myData.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();
            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in myData.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;

            }
            var cmt = myData.DanhGias.Where(p => p.MaBaiViet == id && p.isDeleted == false);
            if (cmt != null)
            {
                ViewBag.slCMT = cmt.Count();
                ViewBag.cmt = cmt;
            }

            else
                ViewBag.checkCMT = -1;

            var lstComment = myData.BinhLuans.Where(p => p.MaBaiViet == id && p.isDeleted==false).OrderByDescending(m=>m.NgayTao);
            
            if(lstComment.Count()!=0)
                ViewBag.comments = lstComment;
            else
                ViewBag.comments = null;
            var reply = myData.PhanHois.ToList();
            if (reply.Count() != 0)
                ViewBag.replys = reply;
            else
                ViewBag.replys = null;
            ViewBag.showAllReplys = 0;
            ViewBag.countReply = 0;
            ViewBag.MaBaiViet = 0;
            ViewBag.countComment = 0;
            ViewBag.showAllCMT = lstComment.Count();

            var baiViet = myData.BaiViets.Where(m => m.MaBaiViet == id).First();
            ViewBag.MaBaiViet = baiViet.MaBaiViet;
            baiViet.LuotXem++;
            UpdateModel(baiViet);
            myData.SubmitChanges();
            var all_baiviet = from tt in myData.DanhGias where tt.MaBaiViet == id && tt.isDeleted == false select tt;
            int sao5 = 0;
            int sao4 = 0;
            int sao3 = 0;
            int sao2 = 0;
            int sao1 = 0;
            int demSao = 0;
            foreach (var item in all_baiviet)
            {
                demSao++;
                if (item.SoSao == 5)
                {
                    sao5++;
                }
                if (item.SoSao == 4)
                {
                    sao4++;
                }
                if (item.SoSao == 3)
                {
                    sao3++;
                }
                if (item.SoSao == 2)
                {
                    sao2++;
                }
                if (item.SoSao == 1)
                {
                    sao1++;
                }
            }
            if (demSao == 0)
            {
                demSao = 1;
                ViewBag.slDanhGia = 0;
            }
            else              
                ViewBag.slDanhGia = demSao;
            ViewBag.sao1 = sao1;ViewBag.phanTram1 = (sao1 * 100) / demSao;
            ViewBag.sao2 = sao2; ViewBag.phanTram2 = (sao2 * 100) / demSao;
            ViewBag.sao3 = sao3; ViewBag.phanTram3 = (sao3 * 100) / demSao;
            ViewBag.sao4 = sao4; ViewBag.phanTram4 = (sao4 * 100) / demSao;
            ViewBag.sao5 = sao5; ViewBag.phanTram5 = (sao5 * 100) / demSao;
            //var comments = myData.BinhLuans.Where(m => m.MaBaiViet == id).ToList();
            //ViewBag.list = comments;

            //check liked cmt
            var check_YeuThich = myData.ChiTietBinhLuans.Where(m=>m.TaiKhoan == User.Identity.Name).ToList();

            ViewBag.checkYT = check_YeuThich;
            ViewBag.tempYT = 0;
            

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

                LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                lichSuHoatDong.Ngay = DateTime.Now;
                lichSuHoatDong.TaiKhoan = User.Identity.Name;
                lichSuHoatDong.MaBaiViet = danhSachYeuThich.MaBaiViet;
                lichSuHoatDong.NoiDung = "Đã yêu thích bài viết";
                lichSuHoatDong.LoaiHoatDong = "yeuThich_Post";

                myData.LichSuHoatDongs.InsertOnSubmit(lichSuHoatDong);
                myData.SubmitChanges();
            }
            else
            {
                var delete = (from s in myData.DanhSachYeuThiches where s.MaBaiViet == id && s.TaiKhoan.Equals(User.Identity.Name) select s).First();
                myData.DanhSachYeuThiches.DeleteOnSubmit(delete);
                baiviet.YeuThich--;
                UpdateModel(baiviet);
                myData.SubmitChanges();
                LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
                lichSuHoatDong.Ngay = DateTime.Now;
                lichSuHoatDong.TaiKhoan = User.Identity.Name;
                lichSuHoatDong.MaBaiViet = baiviet.MaBaiViet;
                lichSuHoatDong.NoiDung = "Đã xóa khỏi danh sách yêu thích";
                lichSuHoatDong.LoaiHoatDong = "xoayeuThich_Post";

                myData.LichSuHoatDongs.InsertOnSubmit(lichSuHoatDong);
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
            ViewBag.status = baiViet.TrangThai;
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
            SetAlert("Edit post success!!", "success");
            ViewBag.TL = baiViet.MaTL;
            return Redirect("https://localhost:44339/Review/quanLyBaiViet");
        }



        [Authorize]
        [HttpPost]
        public ActionResult themDanhGia(int mabaiviet, string comment, int rate)
        {

     
            var D_tk = myData.ChiTietDanhGias.FirstOrDefault(m => m.DanhGia.TaiKhoan.Equals(User.Identity.Name) && m.DanhGia.MaBaiViet == mabaiviet && m.DanhGia.isDeleted == false);
            var lst_DanhGia = myData.DanhGias.Where(m => m.MaBaiViet == mabaiviet && m.isDeleted == false);
            var D_sach = myData.BaiViets.Where(m => m.MaBaiViet == mabaiviet).First();
            if (D_tk == null)
            {
                DanhGia danhGia = new DanhGia();
                danhGia.MaBaiViet = mabaiviet;
                danhGia.NgayTao = DateTime.Now;
                danhGia.TaiKhoan = User.Identity.Name;
                danhGia.SoLike = 0;
                //var soSao = collection["SoSao"];
                var soSao = rate;
                danhGia.SoSao = soSao;
                danhGia.NoiDung = comment;
                danhGia.isDeleted = false;
                myData.DanhGias.InsertOnSubmit(danhGia);
                myData.SubmitChanges();
                ChiTietDanhGia cTDanhGia = new ChiTietDanhGia();
                cTDanhGia.MaBinhLuan = danhGia.MaBinhLuan;
                int? sl = 0;
                foreach (var item in lst_DanhGia)
                {
                    sl += item.SoSao;
                }
                cTDanhGia.TongSao = sl;
                cTDanhGia.DiemDanhGia = (float)cTDanhGia.TongSao / lst_DanhGia.Count();
                myData.ChiTietDanhGias.InsertOnSubmit(cTDanhGia);
                D_sach.SoSao =(float) cTDanhGia.TongSao / lst_DanhGia.Count();
                D_sach.SoLuotDanhGia++;
                UpdateModel(D_sach);
                myData.SubmitChanges();
            }
            else
            {
                D_tk.DanhGia.SoSao = rate;
                D_tk.DanhGia.NoiDung = comment;
                D_tk.DanhGia.NgayTao = DateTime.Now;
                int? sl = 0;
                foreach (var item in lst_DanhGia)
                {
                    sl += item.SoSao;
                }
                D_tk.TongSao = sl;
                UpdateModel(D_tk);
                var tinhSao = myData.ChiTietDanhGias.Where(m => m.DanhGia.MaBaiViet == mabaiviet).AsEnumerable().Last();
                tinhSao.TongSao = sl;
                tinhSao.DiemDanhGia = (float)sl / lst_DanhGia.Count();

                UpdateModel(tinhSao);
                D_sach.SoSao = (float)sl / lst_DanhGia.Count();
                UpdateModel(D_sach);
                myData.SubmitChanges();
            }

            myData.SubmitChanges();
            //var rattings = myData.DanhGias.Where(m => m.MaBaiViet == mabaiviet).OrderByDescending(m => m.NgayTao).ToList();
            //ViewBag.list = rattings;
            var cmt = myData.DanhGias.Where(p => p.MaBaiViet == mabaiviet);
            if (cmt != null)
            {
                
                ViewBag.cmt = cmt;
            }

            else
                ViewBag.checkCMT = -1;
            return PartialView("~/Views/NguoiDung/_RatePartial.cshtml", cmt);
        }
        [Authorize]
        public ActionResult xoaDanhGia(int id)
        {


            var danhGia = myData.DanhGias.Where(m => m.MaBinhLuan == id).First();
            
            int? mabai = danhGia.MaBaiViet;
            ViewBag.ma = mabai;
            if (danhGia != null)
            {
                //hide rate
                danhGia.isDeleted = true;
                UpdateModel(danhGia);
                myData.SubmitChanges();
                //list rate isn't deleted
                var lst_DanhGia = myData.DanhGias.Where(m => m.MaBaiViet == mabai && m.isDeleted == false);
                int? sl = 0;
                foreach (var item in lst_DanhGia)
                {
                    sl += item.SoSao;
                }
                //update diem danh gia
                var tinhSao = myData.ChiTietDanhGias.Where(m => m.DanhGia.MaBaiViet == mabai).AsEnumerable().Last();
                tinhSao.TongSao = sl;
                tinhSao.DiemDanhGia = (float)sl / lst_DanhGia.Count();
                UpdateModel(tinhSao);
                //update post
                var postAfterDelete = myData.BaiViets.Where(m => m.MaBaiViet == mabai).First();
                postAfterDelete.SoSao = (float)sl / lst_DanhGia.Count();
                postAfterDelete.SoLuotDanhGia--;
                UpdateModel(postAfterDelete);
                
                myData.SubmitChanges();
            }
            else
            {
            }
            var comments = myData.DanhGias.Where(m => m.MaBaiViet == mabai && m.isDeleted != null).OrderByDescending(m => m.NgayTao).ToList();

            return PartialView("~/Views/NguoiDung/_RatePartial.cshtml", comments);
        }
        [HttpPost]
        public ActionResult CapNhatDanhGia(int id, string noiDung)
        {
            var binhLuan = myData.DanhGias.Where(m => m.MaBinhLuan == id).First();
            ViewBag.ma = binhLuan.MaBaiViet;
            if (binhLuan != null)
            {
                
                binhLuan.NoiDung = noiDung;
                UpdateModel(binhLuan);
                myData.SubmitChanges();
                
            }
            var commentss = myData.DanhGias.Where(m => m.MaBaiViet == binhLuan.MaBaiViet && m.isDeleted != true).OrderByDescending(m => m.NgayTao).ToList();
            return PartialView("~/Views/NguoiDung/_RatePartial.cshtml", commentss);
        }


        public ActionResult danhSachBaiViet(int? page)
        {
            //so luong thong bao chua doc
            var countNoti = myData.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNum = page ?? 1;
            var allPost = from tt in myData.BaiViets where tt.TrangThai == 1 select tt;

            if(User.Identity.IsAuthenticated)
            {
                var nguoidung = from tt in myData.NguoiDungs where tt.TaiKhoan == User.Identity.Name select tt;
                ViewBag.hinh = nguoidung.First().Hinh;
            }
            
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
            ViewBag.allPosts = allPost;
            //check dsyt
            var check_YeuThich = myData.DanhSachYeuThiches.Where(m => m.TaiKhoan == User.Identity.Name);
            ViewBag.checkYT = check_YeuThich;
            ViewBag.tempYT = 0;
            return View(allPost.ToPagedList(pageNum, pageSize));
        }
        public ActionResult locTheLoai(int id, int? page )
        {
            //check dsyt
            var check_YeuThich = myData.DanhSachYeuThiches.Where(m => m.TaiKhoan == User.Identity.Name);
            ViewBag.checkYT = check_YeuThich;
            ViewBag.tempYT = 0;
            //so luong thong bao chua doc
            var countNoti = myData.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();
            if (page == null) page = 1;
            int pageSize = 6;
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


            var cate = from tt in myData.BaiViets where tt.MaTL == id && tt.TrangThai == 1 select tt;
            return View(cate.ToPagedList(pageNum, pageSize));
        }
        public ActionResult locTheoSao(int? page)
        {
            //check dsyt
            var check_YeuThich = myData.DanhSachYeuThiches.Where(m => m.TaiKhoan == User.Identity.Name);
            ViewBag.checkYT = check_YeuThich;
            ViewBag.tempYT = 0;
            //so luong thong bao chua doc
            var countNoti = myData.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNum = page ?? 1;
            var allPost = from tt in myData.BaiViets.OrderByDescending(m=>m.SoSao).Where(m => m.TrangThai == 1) select tt;

            if (User.Identity.IsAuthenticated)
            {
                var nguoidung = from tt in myData.NguoiDungs where tt.TaiKhoan == User.Identity.Name select tt;
                ViewBag.hinh = nguoidung.First().Hinh;
            }

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


            return View(allPost.ToPagedList(pageNum, pageSize));
        }
        public ActionResult locTheoLuotThich(int? page)
        {
            //check dsyt
            var check_YeuThich = myData.DanhSachYeuThiches.Where(m => m.TaiKhoan == User.Identity.Name);
            ViewBag.checkYT = check_YeuThich;
            ViewBag.tempYT = 0;
            //so luong thong bao chua doc
            var countNoti = myData.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNum = page ?? 1;
            var allPost = from tt in myData.BaiViets.OrderByDescending(m => m.YeuThich).Where(m => m.TrangThai == 1) select tt;

            if (User.Identity.IsAuthenticated)
            {
                var nguoidung = from tt in myData.NguoiDungs where tt.TaiKhoan == User.Identity.Name select tt;
                ViewBag.hinh = nguoidung.First().Hinh;
            }

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


            return View(allPost.ToPagedList(pageNum, pageSize));
        }
        public ActionResult locTheoNgayViet(int? page)
        {
            //check dsyt
            var check_YeuThich = myData.DanhSachYeuThiches.Where(m => m.TaiKhoan == User.Identity.Name);
            ViewBag.checkYT = check_YeuThich;
            ViewBag.tempYT = 0;
            //so luong thong bao chua doc
            var countNoti = myData.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNum = page ?? 1;
            var allPost = from tt in myData.BaiViets.OrderByDescending(m => m.NgayViet).Where(m=>m.TrangThai == 1) select tt;

            if (User.Identity.IsAuthenticated)
            {
                var nguoidung = from tt in myData.NguoiDungs where tt.TaiKhoan == User.Identity.Name select tt;
                ViewBag.hinh = nguoidung.First().Hinh;
            }

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


            return View(allPost.ToPagedList(pageNum, pageSize));
        }


        public ActionResult filter(int id,string option, int? page)
        {
            var check_YeuThich = myData.DanhSachYeuThiches.Where(m => m.TaiKhoan == User.Identity.Name);
            ViewBag.checkYT = check_YeuThich;
            ViewBag.tempYT = 0;
            //so luong thong bao chua doc
            var countNoti = myData.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();
            if (page == null) page = 1;
            int pageSize = 3;
            int pageNum = page ?? 1;
            var allPost = from tt in myData.BaiViets select tt;
            ViewBag.opt = option;
            ViewBag.id = id;

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

            if (option == "Newest")
            {
                var cate = from tt in myData.BaiViets where tt.MaTL == id && tt.TrangThai == 1 orderby tt.NgayViet descending select tt;
                return View(cate.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var cate = from tt in myData.BaiViets where tt.MaTL == id orderby tt.NgayViet ascending select tt;
                return View(cate.ToPagedList(pageNum, pageSize));
            }


            
           
        }
        

        public ActionResult danhSachPhanHoi(int id)
        {
            //so luong thong bao chua doc
            var countNoti = myData.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();
            var user = myData.NguoiDungs.Where(m => m.TaiKhoan == User.Identity.Name).First();
            ViewBag.hinh = user.Hinh;
            ViewBag.cmt = myData.BinhLuans.Where(m => m.MaBinhLuan == id).First();
            var replys = myData.PhanHois.Where(m => m.MaBinhLuan == id).ToList();
            return View(replys);
        }
        public ActionResult danhSachBinhLuan(int id)
        {
            //so luong thong bao chua doc
            var countNoti = myData.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();
            var user = myData.NguoiDungs.Where(m => m.TaiKhoan == User.Identity.Name).First();
            ViewBag.hinh = user.Hinh;
            var lst_cmt = myData.BinhLuans.Where(m => m.MaBaiViet == id && m.isDeleted == false);
            ViewBag.replys = myData.PhanHois.ToList();
            return View(lst_cmt);
        }
        public ActionResult danhSachThongBao(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNum = page ?? 1;
            //so luong thong bao chua doc
            var countNoti = myData.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();
            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in myData.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }
            var all_ThongBao = from tt in myData.ThongBaos where tt.TaiKhoan == User.Identity.Name orderby tt.MaThongBao descending select tt;
            return View(all_ThongBao.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Search(string name, int? page)
        {
            var check_YeuThich = myData.DanhSachYeuThiches.Where(m => m.TaiKhoan == User.Identity.Name);
            ViewBag.checkYT = check_YeuThich;
            ViewBag.tempYT = 0;
            //so luong thong bao chua doc
            var countNoti = myData.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNum = page ?? 1;
            

            if (User.Identity.IsAuthenticated)
            {
                var nguoidung = from tt in myData.NguoiDungs where tt.TaiKhoan == User.Identity.Name select tt;
                ViewBag.hinh = nguoidung.First().Hinh;
            }

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
            var baiViet = myData.BaiViets.Where(m=>m.TenBaiViet.Contains(name) && m.TrangThai == 1);
            ViewBag.keyword = name;
            return View(baiViet.ToPagedList(pageNum, pageSize));
        }


        public ActionResult danhSachBaiVietChuaDuyet(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 3;
            int pageNum = page ?? 1;
            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in myData.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }
            var baiViet = myData.BaiViets.Where(m => m.TrangThai == -1 && m.TaiKhoan == User.Identity.Name);

            return View(baiViet.ToPagedList(pageNum, pageSize));
        }

        public ActionResult danhSachBaiVietBiTuChoi(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 3;
            int pageNum = page ?? 1;
            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in myData.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }
            var baiViet = myData.BaiViets.Where(m => m.TrangThai == -2 && m.TaiKhoan == User.Identity.Name );

            return View(baiViet.ToPagedList(pageNum, pageSize));
        }
        public ActionResult danhSachBaiVietDaDuyet(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 3;
            int pageNum = page ?? 1;
            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in myData.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }
            var baiViet = myData.BaiViets.Where(m => m.TrangThai == 1 && m.TaiKhoan == User.Identity.Name);

            return View(baiViet.ToPagedList(pageNum, pageSize));
        }

    }
}