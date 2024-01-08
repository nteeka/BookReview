using KK_BookStore.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace KK_BookStore.Controllers
{
    public class RoleController : Controller
    {
        // GET: Role
        MyDataDataContext data= new MyDataDataContext();
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationRoleManager _roleManager;

        public RoleController()
        {
        }

        public RoleController(ApplicationRoleManager roleManager)
        {
            RoleManager = roleManager;

        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        // GET: Role
        [Authorize(Roles = "Admin")]

        public ActionResult Index()
        {
            var user = data.NguoiDungs.Where(m => m.TaiKhoan == User.Identity.Name).First();
            ViewBag.Hinh = user.Hinh;
            List<RoleViewModel> list = new List<RoleViewModel>();
            foreach (var role in RoleManager.Roles)
            {
                list.Add(new RoleViewModel(role));
            }
            
            return View(list);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel model)
        {
            var role = new ApplicationRole() { Name = model.Name };
            await RoleManager.CreateAsync(role);
            var getId = db.Roles.Where(m=>m.Name == model.Name).First();
            ChucVu chucVu = new ChucVu();
            chucVu.MaChucVu =getId.Id;
            chucVu.TenChucVu = model.Name;
            data.ChucVus.InsertOnSubmit(chucVu);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));
        }
        [HttpPost]
        public async Task<ActionResult> Edit(RoleViewModel model)
        {
            
            var chucVu = data.ChucVus.Where(m=>m.MaChucVu==model.Id).First();
            chucVu.TenChucVu = model.Name;
            UpdateModel(chucVu);
            data.SubmitChanges();
            var role = new ApplicationRole() { Id = model.Id, Name = model.Name };
            await RoleManager.UpdateAsync(role);

            return RedirectToAction("Index");
        }



        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));
        }
        [HttpPost]
        public async Task<ActionResult> Delete(RoleViewModel model)
        {
            var role = await RoleManager.FindByIdAsync(model.Id);
            var chucVu = data.ChucVus.Where(m => m.MaChucVu == model.Id).First();
            data.ChucVus.DeleteOnSubmit(chucVu);
            data.SubmitChanges();
            await RoleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }


        public ActionResult danhSachNguoiDungTheoRole(string id)
        {
            var all_nguoidung = from tt in data.NguoiDungs where tt.MaChucVu == id select tt;
            var nguoidung = from tt in data.NguoiDungs select tt;
            ViewBag.lstNguoiDung = nguoidung;
            var user = data.NguoiDungs.Where(m => m.TaiKhoan == User.Identity.Name).First();
            ViewBag.Hinh = user.Hinh;
            return View(all_nguoidung);
        }
        //public ActionResult allUser()
        //{
        //    var all_nguoidung = from tt in data.NguoiDungs where tt.ChucVu.TenChucVu == "User" select tt;
               
        //    return View(all_nguoidung);
        //}

        //public ActionResult addToRole(string id)
        //{
        //    var all_nguoidung = from tt in data.NguoiDungs where tt.MaChucVu == id select tt;
        //    var nguoidung = from tt in data.NguoiDungs select tt;
        //    ViewBag.lstNguoiDung = nguoidung;
        //    return View(all_nguoidung);
        //}
    }
}
