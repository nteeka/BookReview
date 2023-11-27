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
        //public List<DanhSachYeuThich> layDanhSach()
        //{
        //    var user = User.Identity.GetUserId();
        //    List<DanhSachYeuThich> lstYeuThich = Session[user] as List<DanhSachYeuThich>;
        //    if (lstYeuThich == null)
        //    {
        //        lstYeuThich = new List<DanhSachYeuThich>();
        //        Session[user] = lstYeuThich;
        //    }
        //    return lstYeuThich;
        //}
        //[Authorize]
        //public ActionResult  themDanhSachYeuThich(int id, string strURL)
        //{

        //    List<DanhSachYeuThich> lstYeuThich = layDanhSach();
        //    DanhSachYeuThich danhsach = lstYeuThich.Find(n => n.maSach == id);
        //    if (danhsach == null)
        //    {
        //        danhsach = new DanhSachYeuThich(id);
        //        Sach sach = data.Saches.FirstOrDefault(n => n.MaSach == id);
        //        sach.SoLike = sach.SoLike +1;
        //        UpdateModel(sach);
        //        data.SubmitChanges();
        //        lstYeuThich.Add(danhsach);             
        //        return Redirect(strURL);
        //    }
        //    else
        //    {

        //        return Redirect(strURL);
        //    }
        //}
        //[Authorize]
        //public ActionResult DanhSachYeuThich()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
        //        ViewBag.hinh = anhDaiDien.First().Hinh;
        //    }
        //    List<DanhSachYeuThich> lstYeuThich = layDanhSach();          
        //    foreach (var item in lstYeuThich)
        //    {
        //        var sach = data.Saches.SingleOrDefault(x => x.MaSach == item.maSach);

        //    }
        //    return View(lstYeuThich);
        //}
        //public ActionResult DanhSachYeuThichPartial()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
        //        ViewBag.hinh = anhDaiDien.First().Hinh;
        //    }
        //    return PartialView();
        //}
        //public ActionResult xoaDanhSachYeuThich(int id)
        //{
        //    List<DanhSachYeuThich> lstYeuThich = layDanhSach();
        //    DanhSachYeuThich danhsach = lstYeuThich.SingleOrDefault(n => n.maSach == id);
        //    if (danhsach != null)
        //    {
        //        Sach sach = data.Saches.FirstOrDefault(n => n.MaSach == id);
        //        sach.SoLike = sach.SoLike -1;
        //        UpdateModel(sach);
        //        data.SubmitChanges();
        //        lstYeuThich.RemoveAll(n => n.maSach == id) ;
        //        return RedirectToAction("DanhSachYeuThich");
        //    }
        //    return RedirectToAction("DanhSachYeuThich");
        //}
        //public ActionResult capNhapDanhSachYeuThich(int id, FormCollection collection)
        //{
        //    List<DanhSachYeuThich> lstYeuThich = layDanhSach();
        //    DanhSachYeuThich danhsach = lstYeuThich.SingleOrDefault(n => n.maSach == id);
        //    return RedirectToAction("DanhSachYeuThich");
        //}
        //public ActionResult xoaTatCaDanhSachYeuThich()
        //{
        //    List<DanhSachYeuThich> lstYeuThich = layDanhSach();
        //    lstYeuThich.Clear();
        //    return RedirectToAction("DanhSachYeuThich");
        //}
        //[Authorize]
        //public ActionResult Index()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
        //        ViewBag.hinh = anhDaiDien.First().Hinh;
        //    }
        //    var danhsach = from tt in data.DanhSachYeuThiches where tt.TaiKhoan == User.Identity.Name  select tt;
        //    return View(danhsach.ToList());
        //}
        [Authorize]
        public ActionResult DanhSachBaiViet(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 3;
            int pageNum = page ?? 1;

            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }
            var danhsach = from tt in data.DanhSachYeuThiches where tt.TaiKhoan == User.Identity.Name select tt;
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
            SetAlert("Delete post from wishlist success!!", "success");
            return RedirectToAction("DanhSachBaiViet");
        }
    }
}