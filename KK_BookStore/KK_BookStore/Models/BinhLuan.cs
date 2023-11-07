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
        [Display(Name = "Tên Khách  Hàng")]
        public String taiKhoan { get; set; }
        [Display(Name = "Mã Sách")]
        public int maSach { get; set; }
        
        

        //public BinhLuan(int MaSach,String idUser)
        //{
        //    taiKhoan = idUser;
        //    maSach = MaSach;
        //    NguoiDung user = data.NguoiDungs.Single(n => n.TaiKhoan == taiKhoan);
        //    Sach sach = data.Saches.Single(n => n.MaSach == maSach);                       
        //}


    }
}