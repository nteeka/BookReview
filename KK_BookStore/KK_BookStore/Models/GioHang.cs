using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace KK_BookStore.Models
{
    public class GioHang
    {
        MyDataDataContext data = new MyDataDataContext();
        [Display(Name = "Mã Sách")]
        public int maSach { get; set; }

        [Display(Name = "Tên Sách")]
        public string tenSach { get; set; }

        [Display(Name = "Ảnh bìa")]
        public string hinh { get; set; }
        [Display(Name = "Giá bán")]
        public double giaBan { get; set; }
        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }
        [Display(Name = "Số lượng")]
        public int soLuong { get; set; }
        [Display(Name = "Thành tiền")]
        public double thanhTien
        {
            get { return soLuong * giaBan; }
        }
        public GioHang(int id)
        {
            maSach = id;
            Sach sach = data.Saches.Single(n => n.MaSach == maSach);
            tenSach = sach.TenSach;
            hinh = sach.HinhAnh;
            MoTa = sach.MoTa;
            giaBan = double.Parse(sach.DonGia.ToString());
            soLuong = 1;
        }
    }
}