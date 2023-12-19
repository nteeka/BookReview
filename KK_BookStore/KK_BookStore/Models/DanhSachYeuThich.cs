//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Web;
//using System.Xml.Linq;
//using Twilio.Rest.Autopilot.V1.Assistant;

//namespace KK_BookStore.Models
//{
//    public class DanhSachYeuThich
//    {
//        MyDataDataContext data = new MyDataDataContext();
//        [Display(Name = "Mã Sách")]
//        public int maSach { get; set; }

//        [Display(Name = "Tên Sách")]
//        public string tenSach { get; set; }

//        [Display(Name = "Ảnh bìa")]
//        public string hinh { get; set; }
//        //[Display(Name = "Số like")]
//        //public int? SoLike { get; set; }
//        public DanhSachYeuThich(int id)
//        {
//            maSach = id;
//            Sach sach = data.Saches.Single(n => n.MaSach == maSach);
//            tenSach = sach.TenSach;
//            hinh = sach.HinhAnh;
//            //SoLike++;
//            //sach.SoLike = SoLike;           
//        }
//    }
//}