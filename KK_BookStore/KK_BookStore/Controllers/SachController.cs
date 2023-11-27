using KK_BookStore.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.PeerToPeer;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace KK_BookStore.Controllers
{
    public class SachController : Controller
    {
        // GET: Sach
        MyDataDataContext data = new MyDataDataContext();
        //public ActionResult Index(int? page)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
        //        ViewBag.hinh = anhDaiDien.First().Hinh;
        //    }
        //    if (page == null) page = 1;
        //    var all_sach = (from s in data.Saches where s.SoLuongSach > 0 select s).OrderBy(m => m.MaSach);
        //    int pageSize = 6;
        //    int pageNum = page ?? 1;
        //    var alltheloai = (from tl in data.TheLoais.Distinct() select tl);
        //    List<TheLoai> lstTheLoai = new List<TheLoai>();
        //    foreach (var tl in alltheloai)
        //    {
        //        lstTheLoai.Add(tl);
        //    }
        //    ViewBag.TheLoais = lstTheLoai;
        //    var sachGiamGia = from s in data.SachGiamGias select s;
        //    ViewBag.sachGiamGia = sachGiamGia;
        //    return View(all_sach.ToPagedList(pageNum, pageSize));
        //    //return View();

        //}
        //public ActionResult Detail(int id)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
        //        ViewBag.hinh = anhDaiDien.First().Hinh;
        //    }
            
        //    var D_sach = data.Saches.Where(m => m.MaSach == id).First();
        //    var lstSach = data.Saches.Where(m=>m.MaTL == D_sach.MaTL && m.MaSach!=id);
        //    var soBinhLuan = data.BinhLuans.Where(m => m.MaSach == id);
        //    ViewBag.danhSachBinhLuan = soBinhLuan;
        //    ViewBag.soBinhLuan = soBinhLuan.Count();
        //    ViewBag.goiY = lstSach;
        //    ViewBag.sl = 0;
        //    return View(D_sach);
        //}
        //public ActionResult TimKiem(string searching)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
        //        ViewBag.hinh = anhDaiDien.First().Hinh;
        //    }
        //    var sachGiamGia = from s in data.SachGiamGias select s;
        //    ViewBag.sachGiamGia = sachGiamGia;
        //    return View(data.Saches.Where(x => x.TenSach.ToLower().Contains(searching.ToLower()) || searching == null));
        //}
        //public ActionResult LocTheLoai(int maTheLoai)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
        //        ViewBag.hinh = anhDaiDien.First().Hinh;
        //    }
        //    var sachGiamGia = from s in data.SachGiamGias select s;
        //    ViewBag.sachGiamGia = sachGiamGia;
        //    var alltheloai = (from tl in data.TheLoais.Distinct() select tl);
        //    List<TheLoai> lstTheLoai = new List<TheLoai>();
        //    foreach (var tl in alltheloai)
        //    {
        //        lstTheLoai.Add(tl);
        //    }
        //    ViewBag.TheLoais = lstTheLoai;
        //    return View(data.Saches.Where(x => x.MaTL == maTheLoai));
        //}

        //public ActionResult LocGia(int chon)
        //{
            
        //    var sachGiamGia = from s in data.SachGiamGias select s;
        //    ViewBag.sachGiamGia = sachGiamGia;
        //    var alltheloai = (from tl in data.TheLoais.Distinct() select tl);
        //    List<TheLoai> lstTheLoai = new List<TheLoai>();
        //    foreach (var tl in alltheloai)
        //    {
        //        lstTheLoai.Add(tl);
        //    }
        //    ViewBag.TheLoais = lstTheLoai;
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
        //        ViewBag.hinh = anhDaiDien.First().Hinh;
        //    }
        //    if (chon==1)
        //    {
        //        var giaSach1 = (from tl in data.Saches where tl.DonGia < 100000 select tl);
        //        List<Sach> lstSach = new List<Sach>();
        //        foreach (var tl in giaSach1)
        //        {
        //            lstSach.Add(tl);
        //        }
        //        return View(lstSach.ToList());
        //    }
        //    if(chon==2)
        //    {
        //        var giaSach1 = (from tl in data.Saches where tl.DonGia > 100000 && tl.DonGia<200000 select tl);
        //        List<Sach> lstSach = new List<Sach>();
        //        foreach (var tl in giaSach1)
        //        {
        //            lstSach.Add(tl);
        //        }
        //        return View(lstSach.ToList());
        //    }
        //    if (chon == 3)
        //    {
        //        var giaSach1 = (from tl in data.Saches where tl.DonGia > 200000 select tl);
        //        List<Sach> lstSach = new List<Sach>();
        //        foreach (var tl in giaSach1)
        //        {
        //            lstSach.Add(tl);
        //        }
        //        return View(lstSach.ToList());
        //    }
            
        //    return View();
        //}

        //public ActionResult LocGiaTheLoai(int chon,int cate)
        //{

        //    var sachGiamGia = from s in data.SachGiamGias select s;
        //    ViewBag.sachGiamGia = sachGiamGia;
        //    var alltheloai = (from tl in data.TheLoais.Distinct() select tl);
        //    List<TheLoai> lstTheLoai = new List<TheLoai>();
        //    foreach (var tl in alltheloai)
        //    {
        //        lstTheLoai.Add(tl);
        //    }
        //    ViewBag.TheLoais = lstTheLoai;
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
        //        ViewBag.hinh = anhDaiDien.First().Hinh;
        //    }
        //    if (chon == 1)
        //    {
        //        var giaSach1 = (from tl in data.Saches where tl.DonGia < 100000 && tl.MaTL == cate select tl);
        //        List<Sach> lstSach = new List<Sach>();
        //        foreach (var tl in giaSach1)
        //        {
        //            lstSach.Add(tl);
        //        }
        //        return View(lstSach.ToList());
        //    }
        //    if (chon == 2)
        //    {
        //        var giaSach1 = (from tl in data.Saches where (tl.DonGia > 100000 && tl.DonGia < 200000) && tl.MaTL == cate select tl);
        //        List<Sach> lstSach = new List<Sach>();
        //        foreach (var tl in giaSach1)
        //        {
        //            lstSach.Add(tl);
        //        }
        //        return View(lstSach.ToList());
        //    }
        //    if (chon == 3)
        //    {
        //        var giaSach1 = (from tl in data.Saches where tl.DonGia > 200000 && tl.MaTL == cate select tl);
        //        List<Sach> lstSach = new List<Sach>();
        //        foreach (var tl in giaSach1)
        //        {
        //            lstSach.Add(tl);
        //        }
        //        return View(lstSach.ToList());
        //    }

        //    return View();
        //}


        ////create(chon the loai)
        ////public ActionResult LocTheLoai(string cate)
        ////{
        ////    List<SelectListItem> list = new List<SelectListItem>();
        ////    var alltheloai = (from tl in data.TheLoais select tl);
        ////    foreach (var role in alltheloai)
        ////        list.Add(new SelectListItem() { Value = role.TenTL, Text = role.TenTL });
        ////    ViewBag.Roles = list;
        ////    return View(data.Saches.Where(x => x.TheLoai.TenTL.ToLower().Equals(cate.ToLower()) || cate == null));
        ////}
        ////view
        ////        @using(Html.BeginForm("LocTheLoai", "Sach", FormMethod.Get))
        ////{
        ////    <div class="col-md-10" style="margin-left:10px;width:150px">
        ////        <p><b>Chọn tl</b></p>
        ////        @Html.DropDownListFor(m => m., new SelectList(ViewBag.Roles, "Value", "Text", ViewBag.SelectedItem), new { @class = "form-control" })
        ////    </div>
        ////}
        //[Authorize]
        //public ActionResult BinhLuan(int id)
        //{
        //    var D_sach = data.BinhLuans.Where(m => m.MaSach == id).First();
        //    //ViewBag.id = id;
        //    ViewBag.MaSach = id;
        //    return View(D_sach);             
        //}

        ////[HttpPost]
        ////public ActionResult BinhLuan(FormCollection collection, BinhLuan tl)
        ////{

        ////    var ten = collection["NoiDung"];
        ////    if (string.IsNullOrEmpty(ten))
        ////    {
        ////        ViewData["Error"] = "Don't empty!";
        ////    }
        ////    else
        ////    {
        ////        tl.NoiDung = ten;
        ////        data.BinhLuans.InsertOnSubmit(tl);
        ////        data.SubmitChanges();
        ////        return RedirectToAction("Index");
        ////    }
        ////    return this.BinhLuan(ViewBag.id);

        ////}

        ////public ActionResult themBinhLuan(int masach,string username,string noidung, string strURL)
        ////{
        ////    var comment = new BinhLuan();
        ////    comment.MaSach = masach;
        ////    comment.TaiKhoan = username;
        ////    comment.NoiDung = noidung;
        ////    UpdateModel(comment);
        ////    data.BinhLuans.InsertOnSubmit(comment);
        ////    data.SubmitChanges();
        ////    return Redirect(strURL);
        ////}
        //[Authorize]
        //public ActionResult themBinhLuan(int masach,string noidung, int? rate)
        //{

        //    //var comment = new BinhLuan();
        //    //comment.MaSach = masach;
        //    //comment.TaiKhoan = User.Identity.Name;
        //    //comment.NoiDung = noidung;
        //    //comment.NgayTao = DateTime.Now.ToString("dd/MM/yyyy");
        //    //ViewBag.MaSach = masach;
        //    //UpdateModel(comment);
        //    //data.BinhLuans.InsertOnSubmit(comment);
        //    //data.SubmitChanges();
        //    //return Redirect("https://localhost:44339/Sach/danhSachBinhLuan/" + masach);
            
        //    var D_tk = data.CTBinhLuans.FirstOrDefault(m => m.BinhLuan.TaiKhoan.Equals(User.Identity.Name) && m.BinhLuan.MaSach == masach);
        //    var lst_DanhGia = data.BinhLuans.Where(m => m.MaSach == masach);
        //    var D_sach = data.Saches.Where(m => m.MaSach == masach).First();
        //    if (D_tk == null)
        //    {
        //        BinhLuan danhGia = new BinhLuan();
        //        danhGia.MaSach = masach;
        //        danhGia.NgayTao = DateTime.Now;
        //        danhGia.TaiKhoan = User.Identity.Name;
        //        danhGia.SoLike = 0;
        //        //var soSao = collection["SoSao"];
        //        var soSao = rate;
        //        danhGia.SoSao = soSao;
        //        danhGia.NoiDung = noidung;
        //        data.BinhLuans.InsertOnSubmit(danhGia);
        //        data.SubmitChanges();
        //        CTBinhLuan cTDanhGia = new CTBinhLuan();
        //        cTDanhGia.MaBinhLuan = danhGia.MaBinhLuan;
        //        int? sl = 0;
        //        foreach (var item in lst_DanhGia)
        //        {
        //            sl += item.SoSao;
        //        }
        //        cTDanhGia.TongSao = sl;
        //        cTDanhGia.DiemDanhGia = Math.Round((float)cTDanhGia.TongSao / lst_DanhGia.Count(),2);
        //        data.CTBinhLuans.InsertOnSubmit(cTDanhGia);
        //        D_sach.SoSao =Math.Round((float)cTDanhGia.TongSao / lst_DanhGia.Count(),2) ;
        //        UpdateModel(D_sach);
        //        data.SubmitChanges();
        //    }
        //    else
        //    {
        //        //D_tk.DanhGia.SoSao = int.Parse(collection["SoSao"]);
        //        D_tk.BinhLuan.SoSao = rate;
        //        D_tk.BinhLuan.NoiDung = noidung;
        //        D_tk.BinhLuan.NgayTao = DateTime.Now;
        //        int? sl = 0;
        //        foreach (var item in lst_DanhGia)
        //        {
        //            sl += item.SoSao;
        //        }
        //        D_tk.TongSao = sl;
        //        //D_tk.DiemDanhGia = D_tk.TongSao / lst_DanhGia.Count();
        //        UpdateModel(D_tk);
        //        var tinhSao = data.CTBinhLuans.Where(m => m.BinhLuan.MaSach == masach).AsEnumerable().Last();
        //        tinhSao.TongSao = sl;
        //        tinhSao.DiemDanhGia =Math.Round((float)sl / lst_DanhGia.Count(),2) ;
                
        //        UpdateModel(tinhSao);
        //        D_sach.SoSao =Math.Round((float)sl / lst_DanhGia.Count(),2) ;
        //        UpdateModel(D_sach);
        //        data.SubmitChanges();
        //    }

        //    data.SubmitChanges();
        //    return Redirect("https://localhost:44339/Sach/danhSachBinhLuan/" + masach);
        //}
        //[Authorize]
        //public ActionResult danhSachBinhLuan(int? id)
        //{
        //    //if (User.Identity.IsAuthenticated)
        //    //{
        //    //    var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
        //    //    ViewBag.hinh = anhDaiDien.First().Hinh;
        //    //}
        //    ////var D_sach = data.BinhLuans.Where(m => m.MaSach == masach);
        //    //var D_sach = (from s in data.BinhLuans where s.MaSach == id orderby s.MaBinhLuan descending select s);
        //    //ViewBag.MaSach = id;
        //    //var sach = (from s in data.Saches where s.MaSach == id  select s);
        //    //ViewBag.hinhSach = sach.First().HinhAnh;
        //    //ViewBag.tenSach = sach.First().TenSach;
        //    //ViewBag.moTa = sach.First().MoTa;
        //    //ViewBag.tenTacGia = sach.First().TenTacGia;
        //    //ViewBag.soLike = sach.First().SoLike;
        //    //ViewBag.donGia = sach.First().DonGia;
        //    //return View(D_sach.ToList());
        //    var soSa0 = data.Saches.Where(m => m.MaSach == id).First();
        //    ViewBag.soSao = Math.Round((double)soSa0.SoSao, 1);
        //    var lst_DanhGia = data.BinhLuans.Where(m => m.MaSach == id);
        //    ViewBag.soLuongNguoiDanhGia = lst_DanhGia.Count();
        //    //var D_sach = data.BinhLuans.Where(m => m.MaSach == masach);
        //    var D_sach = (from s in data.BinhLuans where s.MaSach == id orderby s.MaBinhLuan descending select s);
        //    ViewBag.MaSach = id;
        //    var sach = (from s in data.Saches where s.MaSach == id select s);
        //    ViewBag.hinhSach = sach.First().HinhAnh;
        //    ViewBag.tenSach = sach.First().TenSach;
        //    ViewBag.tenTacGia = sach.First().TenTacGia;
        //    ViewBag.donGia = sach.First().DonGia;
        //    ViewBag.namXuatBan = sach.First().NamXuatBan;
        //    ViewBag.nhaXuatBan = sach.First().NhaXuatBan;
            
        //    return View(D_sach.ToList());

        //}

        //[Authorize]
        //public ActionResult xoaBinhLuan(int id)
        //{
        //    var ctbinhluan = (from s in data.CTBinhLuans where s.MaBinhLuan == id select s).First();
        //    data.CTBinhLuans.DeleteOnSubmit(ctbinhluan);
        //    var binhluan = (from s in data.BinhLuans where s.MaBinhLuan == id select s).First();
        //    int? temp = binhluan.MaSach;
        //    data.BinhLuans.DeleteOnSubmit(binhluan);
        //    data.SubmitChanges();
        //    var D_sach = data.Saches.Where(m => m.MaSach == temp).First();          
        //    var lst_DanhGia = data.BinhLuans.Where(m => m.MaSach == binhluan.MaSach);
        //    int? sl = 0;
        //    foreach (var item in lst_DanhGia)
        //    {
        //        sl += item.SoSao;
        //    }
        //    D_sach.SoSao = (float)sl / lst_DanhGia.Count();
        //    UpdateModel(D_sach);     
        //    data.SubmitChanges();         
        //    return Redirect("https://localhost:44339/Sach/danhSachBinhLuan/" + temp);
        //}
        //[Authorize]
        //public ActionResult thichBinhLuan(int id)
        //{
        //    var binhLuan = (from s in data.BinhLuans where s.MaBinhLuan == id select s).First();
        //    int? temp = binhLuan.MaSach;
        //    var check = data.CTLikeBinhLuans.Where(m => m.MaBinhLuan == id && m.TaiKhoan.Equals(User.Identity.Name)).FirstOrDefault();
        //    //th chua like
        //    if (check == null)
        //    {
        //        CTLikeBinhLuan cTLike = new CTLikeBinhLuan();
        //        cTLike.MaBinhLuan = (int)id;
        //        cTLike.TaiKhoan = User.Identity.Name;
        //        UpdateModel(cTLike);
        //        data.CTLikeBinhLuans.InsertOnSubmit(cTLike);
        //        binhLuan.SoLike++;
        //        UpdateModel(binhLuan);
        //        data.SubmitChanges();
        //    }
        //    else
        //    {             
        //        var delete = (from s in data.CTLikeBinhLuans where s.MaBinhLuan == id && s.TaiKhoan.Equals(User.Identity.Name) select s).First();
        //        data.CTLikeBinhLuans.DeleteOnSubmit(delete);
        //        binhLuan.SoLike--;
        //        UpdateModel(binhLuan);
        //        data.SubmitChanges();
        //    }
        //    return Redirect("https://localhost:44339/Sach/danhSachBinhLuan/" + temp);
        //}
        ////
        ////[Authorize]
        ////public ActionResult themSachGiamGia(int? id)
        ////{
        ////    var D_sach = (from s in data.Saches where s.MaSach == id select s).First();
        ////    var sale = new SachGiamGia();
        ////    sale.MaSach = D_sach.MaSach;
        ////    sale.GiaCu = D_sach.DonGia;
        ////    sale.GiaMoi = sale.GiaCu - (sale.GiaCu* (sale.PhanTram/100));
        ////    UpdateModel(sale);
        ////    data.SachGiamGias.InsertOnSubmit(sale);
        ////    data.SubmitChanges();
        ////    return Redirect("https://localhost:44339/Admin/quanLySach");
        ////}
        //////[Authorize]
        ////public ActionResult XoaSachGiamGia(int? id)
        ////{
        ////    var D_sach = (from s in data.SachGiamGias where s.MaSach == id select s).First();        
        ////    data.SachGiamGias.DeleteOnSubmit(D_sach);
        ////    data.SubmitChanges();
        ////    return Redirect("https://localhost:44339/Home/Index");
        ////}
        //public ActionResult danhSachGiamGia(int? page)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
        //        ViewBag.hinh = anhDaiDien.First().Hinh;
        //    }
        //    if (page == null) page = 1;
        //    int pageSize = 6;
        //    int pageNum = page ?? 1;
        //    var D_sach = (from s in data.SachGiamGias select s);
        //    return View(D_sach.ToPagedList(pageNum, pageSize));
        //}
        //public ActionResult DanhGia(int id)
        //{
            
        //    var D_sach = (from s in data.Saches where s.MaSach == id select s);
        //    return View(D_sach);
        //}

    }
}