using KK_BookStore.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KK_BookStore.Controllers
{
    public class DanhSachYeuThichController : Controller
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
        [Authorize]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
                ViewBag.hinh = anhDaiDien.First().Hinh;
            }
            var danhsach = from tt in data.DanhSachYeuThiches where tt.TaiKhoan == User.Identity.Name  select tt;
            return View(danhsach.ToList());
        }
        [Authorize]
        public ActionResult themDanhSachYeuThich(int id, string strURL)
        {
            //var D_sach1 = (from s in data.DanhSachYeuThiches where s.MaSach == id && s.TaiKhoan == User.Identity.Name select s).FirstOrDefault();
            //if (D_sach1 != null)
            //{
            //    return Redirect(strURL);

            //}
            //var capnhapLike = from tt in data.Saches where tt.MaSach == id select tt;            
            //capnhapLike.First().SoLike++;
            //UpdateModel(capnhapLike);


            //DanhSachYeuThich danhsach = new DanhSachYeuThich();
            //danhsach.MaSach = id;
            //danhsach.TaiKhoan = User.Identity.Name;
            //data.DanhSachYeuThiches.InsertOnSubmit(danhsach);
            //data.SubmitChanges();
                
            
            return Redirect(strURL);

        }
        public ActionResult xoaDanhSachYeuThich(int id)
        {
            //var capnhapunLike = from tt in data.Saches where tt.MaSach == id select tt;
            //capnhapunLike.First().SoLike--;
            //UpdateModel(capnhapunLike);

            //var sach = from tt in data.DanhSachYeuThiches where tt.MaSach == id && tt.TaiKhoan==User.Identity.Name select tt;
            //data.DanhSachYeuThiches.DeleteOnSubmit(sach.First());
            //data.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}