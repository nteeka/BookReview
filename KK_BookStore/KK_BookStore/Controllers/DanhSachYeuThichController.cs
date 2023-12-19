using KK_BookStore.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace KK_BookStore.Controllers
{
    public class DanhSachYeuThichController : BaseController
    {
       
        MyDataDataContext data = new MyDataDataContext();
        
        [Authorize]
        public ActionResult DanhSachBaiViet(int? page)
        {
            //so luong thong bao chua doc
            var countNoti = data.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();

            if (page == null) page = 1;
            int pageSize = 3;
            int pageNum = page ?? 1;

            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }
            var danhsach = from tt in data.DanhSachYeuThiches where tt.TaiKhoan == User.Identity.Name select tt;
            var danhSachFolow = from tt in data.FollowDetails where tt.TaiKhoan == User.Identity.Name select tt;
            ViewBag.folow = danhSachFolow.Count();
            ViewBag.post = danhsach.Count();
            return View(danhsach.ToPagedList(pageNum, pageSize));
        }
        
        public ActionResult xoaDanhSachYeuThich(int id)
        {
            var baiviet = (from s in data.BaiViets where s.MaBaiViet == id select s).First();

            var delete = (from s in data.DanhSachYeuThiches where s.MaBaiViet == id && s.TaiKhoan.Equals(User.Identity.Name) select s).First();
            data.DanhSachYeuThiches.DeleteOnSubmit(delete);
            baiviet.YeuThich--;
            UpdateModel(baiviet);
            data.SubmitChanges();
            LichSuHoatDong lichSuHoatDong = new LichSuHoatDong();
            lichSuHoatDong.Ngay = DateTime.Now;
            lichSuHoatDong.TaiKhoan = User.Identity.Name;
            lichSuHoatDong.MaBaiViet = baiviet.MaBaiViet;
            lichSuHoatDong.NoiDung = "Đã xóa khỏi danh sách yêu thích";
            lichSuHoatDong.LoaiHoatDong = "xoayeuThich_Post";

            data.LichSuHoatDongs.InsertOnSubmit(lichSuHoatDong);
            data.SubmitChanges();
            SetAlert("Delete post from wishlist success!!", "success");
            return RedirectToAction("DanhSachBaiViet");
        }


        [Authorize]
        public ActionResult DanhSachTacGia(int? page)
        {
            //so luong thong bao chua doc
            var countNoti = data.ThongBaos.Where(m => m.TaiKhoan == User.Identity.Name && m.TrangThai == 0);
            ViewBag.soLuongTBChuaDoc = countNoti.Count();

            if (page == null) page = 1;
            int pageSize = 3;
            int pageNum = page ?? 1;

            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }
            var danhsachPost = from tt in data.DanhSachYeuThiches where tt.TaiKhoan == User.Identity.Name select tt;
            ViewBag.post = danhsachPost.Count();


            var danhsach = from tt in data.FollowDetails where tt.TaiKhoan == User.Identity.Name select tt;
            
            return View(danhsach.ToPagedList(pageNum, pageSize));
        }

    }
}