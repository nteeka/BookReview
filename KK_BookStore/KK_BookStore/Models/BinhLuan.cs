using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace KK_BookStore.Models
{
    
    public partial class BinhLuan
    {
        
        MyDataDataContext data = new MyDataDataContext();
        [Display(Name = "Mã Binh Luan")]
        public int MaBinhLuan { get; set; }
        [Display(Name = "Nội dung")]
        public String noiDung { get; set; }
        [Display(Name = "Tên tài khoản")]
        public String taiKhoan { get; set; }
        [Display(Name = "Mã Bài viết")]
        public int maBaiViet { get; set; }

        public DateTime NgayTao { get; set; }

        public int soLike { get; set; }



        //public BinhLuan(int MaSach,String idUser)
        //{
        //    taiKhoan = idUser;
        //    maSach = MaSach;
        //    NguoiDung user = data.NguoiDungs.Single(n => n.TaiKhoan == taiKhoan);
        //    Sach sach = data.Saches.Single(n => n.MaSach == maSach);                       
        //}


    }
}