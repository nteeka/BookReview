//using KK_BookStore.Models;
//using Microsoft.AspNet.Identity;
//using PayPal.Api;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Twilio.TwiML.Voice;

//namespace KK_BookStore.Controllers
//{
//    public class GioHangController : Controller
//    {
//        // GET: GioHang
//        MyDataDataContext data = new MyDataDataContext();

//        public ActionResult Index()
//        {
//            return View();
//        }
//        public  List<GioHang> layGioHang()
//        {
//            var user = User.Identity.GetUserName();
//            List<GioHang> lstGioHang = Session[user] as List<GioHang>;
//            if (lstGioHang == null)
//            {
//                lstGioHang = new List<GioHang>();
//                Session[user] = lstGioHang;
//            }
//            return lstGioHang;
//        }
//        //them sp vao gio hang
//        [Authorize]
//        public ActionResult themGioHang(int id, string strURL)
//        {

//            List<GioHang> lstGioHang = layGioHang();
//            GioHang sanpham = lstGioHang.Find(n => n.maSach == id);
//            if (sanpham == null)
//            {
//                sanpham = new GioHang(id);
//                lstGioHang.Add(sanpham);
//                return Redirect(strURL);
//            }
//            else
//            {
//                sanpham.soLuong++;
//                return Redirect(strURL);
//            }
//        }
//        private int tongSoLuong()
//        {
//            var user = User.Identity.GetUserName();
//            int tong = 0;
//            List<GioHang> lstGioHang = Session[user] as List<GioHang>;
//            if (lstGioHang != null)
//            {
//                tong = lstGioHang.Sum(n => n.soLuong);
//            }
//            return tong;
//        }
//        private int tongSoLuongSanPham()
//        {
//            var user = User.Identity.GetUserName();
//            int tong = 0;
//            List<GioHang> lstGioHang = Session[user] as List<GioHang>;
//            if (lstGioHang != null)
//            {
//                tong = lstGioHang.Count;
//            }
//            return tong;
//        }
//        private double tongTien()
//        {
//            var user = User.Identity.GetUserName();
//            double tong = 0;
//            List<GioHang> lstGioHang = Session[user] as List<GioHang>;
//            if (lstGioHang != null)
//            {
//                tong = lstGioHang.Sum(n => n.thanhTien);
//            }
//            return tong;
//        }
//        private double tongTienUSD()
//        {
//            var user = User.Identity.GetUserName();
//            double tong = 0;
//            List<GioHang> lstGioHang = Session[user] as List<GioHang>;
//            if (lstGioHang != null)
//            {
//                tong = lstGioHang.Sum(n => n.thanhTien);
//            }
//            return Math.Round(tong/23500,2);
//        }
//        [Authorize]
//        public ActionResult GioHang()
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
//                ViewBag.hinh = anhDaiDien.First().Hinh;
//            }
//            List<GioHang> lstGioHang = layGioHang();
//            ViewBag.Tongsoluong = tongSoLuong();
//            ViewBag.Tongtien = tongTien();
//            ViewBag.Tongsoluongsanpham = tongSoLuongSanPham();
//            foreach (var item in lstGioHang)
//            {
//                var sach = data.Saches.SingleOrDefault(x => x.MaSach == item.maSach);
//                ViewBag.SoLuongTon = sach.SoLuongSach;
//            }
//            return View(lstGioHang);
//        }
//        //[ChildActionOnly]
//        public PartialViewResult GioHangPartial()
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
//                ViewBag.hinh = anhDaiDien.First().Hinh;
//            }
//            ViewBag.Tongsoluong = tongSoLuong();
//            ViewBag.Tongtien = tongTien();
//            ViewBag.Tongsoluongsanpham = tongSoLuongSanPham();
//            var user = User.Identity.Name;
//            var cart = Session[user] as List<GioHang>;
//            var list = new List<GioHang>();
//            if (cart != null)
//            {
//                list = (List<GioHang>)cart;
//            }

//            return PartialView(list);
//        }
//        public ActionResult xoaGioHang(int id)
//        {
//            List<GioHang> lstGioHang = layGioHang();
//            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.maSach == id);
//            if (sanpham != null)
//            {
//                lstGioHang.RemoveAll(n => n.maSach == id);
//                return RedirectToAction("GioHang");
//            }
//            return RedirectToAction("GioHang");
//        }
//        public ActionResult capNhapGioHang(int id, FormCollection collection)
//        {
//            List<GioHang> lstGioHang = layGioHang();
//            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.maSach == id);
//            if (sanpham != null)
//            {
//                sanpham.soLuong = int.Parse(collection["txtsoLg"].ToString());
//            }
//            /* capNhapSoLuongTon(id);*/
//            return RedirectToAction("GioHang");
//        }
//        public ActionResult xoaTatCaGioHang()
//        {
//            List<GioHang> lstGioHang = layGioHang();
//            lstGioHang.Clear();
//            return RedirectToAction("GioHang");
//        }

//        [HttpGet]
        
//        public ActionResult XacNhanDonHang()
//        {
//            // Lấy danh sách sản phẩm trong giỏ hàng
//            if (User.Identity.IsAuthenticated)
//            {
//                var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
//                ViewBag.hinh = anhDaiDien.First().Hinh;
//            }
//            List<GioHang> lstGioHang = layGioHang();
//            var nguoidung = data.NguoiDungs.Where(m => m.TaiKhoan.Equals(User.Identity.Name)).First();
//            if (nguoidung.DiaChi == null || nguoidung.SDT == null)
//            {
//                ViewBag.KiemTra = 0;
//            }
//            else
//            {
//                ViewBag.KiemTra = 1;
//            }

//            //Kiểm tra nếu giỏ hàng trống thì quay về trang chủ
//            if (lstGioHang.Count == 0)
//            {
//                return RedirectToAction("GioHang", "GioHang");
//            }
//            data.SubmitChanges();
//            ViewBag.Tongsoluongsanpham = tongSoLuongSanPham();
//            ViewBag.Tongsoluong = tongSoLuong();
//            ViewBag.Tongtien = tongTien();
//            // Cập nhật số lượng sản phẩm trong cơ sở dữ liệu
            
//            return View(lstGioHang);
//        }
//        public ActionResult lapHoaDon()
//        {
            
//            List<GioHang> lstGioHang = layGioHang();

//            //Kiểm tra nếu giỏ hàng trống thì quay về trang chủ
            
//            data.SubmitChanges();
//            ViewBag.Tongsoluongsanpham = tongSoLuongSanPham();
//            ViewBag.Tongsoluong = tongSoLuong();
//            ViewBag.Tongtien = tongTien();
//            // Cập nhật số lượng sản phẩm trong cơ sở dữ liệu
//            foreach (var item in lstGioHang)
//            {
//                var sach = data.Saches.SingleOrDefault(s => s.MaSach == item.maSach);
//                if (sach != null)
//                {
//                    sach.SoLuongSach -= item.soLuong;
//                }
//            }
//            data.SubmitChanges();
//            HoaDon hoadon = new HoaDon();
//            List<GioHang> giohang = layGioHang();
//            hoadon.TaiKhoan = User.Identity.Name;
//            hoadon.NgayTao =DateTime.Now;
//            //hoadon.TinhTrangDonHang = false;
//            hoadon.TinhTrangThanhToan = false;
//            hoadon.TinhTrang = null;
//            data.HoaDons.InsertOnSubmit(hoadon);
//            data.SubmitChanges();
//            foreach (var item in giohang)
//            {
//                CTHoaDon cthd = new CTHoaDon();
//                cthd.MaHD = hoadon.MaHD;
//                cthd.MaSach = item.maSach;
//                cthd.SoLuongMua = item.soLuong;
//                cthd.ThanhTien = item.thanhTien;
//                data.SubmitChanges();
//                data.CTHoaDons.InsertOnSubmit(cthd);
//            }
//            data.SubmitChanges();
//            var strSanPham = "";
//            double thanhtien = 0;

//            foreach (var item in giohang)
//            {
//                strSanPham += "<tr>";
//                strSanPham += "<td>" + item.tenSach + "</td>";
//                strSanPham += "<td>" + item.soLuong + "</td>";
//                strSanPham += "<td>" + item.giaBan + "</td>";
//                strSanPham += "</tr>";
//                thanhtien += item.giaBan * item.soLuong;

//            }
//            var tongtien = thanhtien;
//            string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send2.html"));
//            contentCustomer = contentCustomer.Replace("{{MaDon}}", hoadon.MaHD.ToString());
//            contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
//            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", hoadon.NguoiDung.HoTen);
//            contentCustomer = contentCustomer.Replace("{{SDT}}", hoadon.NguoiDung.SDT);
//            contentCustomer = contentCustomer.Replace("{{Email}}", hoadon.NguoiDung.Email);
//            contentCustomer = contentCustomer.Replace("{{DiaChi}}", hoadon.NguoiDung.DiaChi);
//            contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));


//            contentCustomer = contentCustomer.Replace("{{ThanhTien}}", thanhtien.ToString());
//            contentCustomer = contentCustomer.Replace("{{TongTien}}", tongtien.ToString());
//            KK_BookStore.Models.sendEmail.SendEmail("K&K BookStore", "Đơn hàng #" + hoadon.MaHD, contentCustomer.ToString(), hoadon.NguoiDung.Email);


//            var user = User.Identity.GetUserName();
//            // Xóa giỏ hàng sau khi đặt hàng thành công
//            Session[user] = null;
//            //return View(lstGioHang);
//            return RedirectToAction("CamOnDatHang", "GioHang");
//        }

//        //public ActionResult dathang(FormCollection collection)
//        //{
//        //    return RedirectToAction("XacNhanDonHang", "GioHang");
//        //}
//        public ActionResult CamOnDatHang()
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
//                ViewBag.hinh = anhDaiDien.First().Hinh;
//            }
//            return View();
//        }
//        [Authorize]
//        public ActionResult LichSuDatHang()
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
//                ViewBag.hinh = anhDaiDien.First().Hinh;
//            }
//            var lichSuDH =  from s  in data.HoaDons  where s.TaiKhoan.Equals(User.Identity.Name)  orderby s.MaHD descending  select s ;
//            return View(lichSuDH.ToList());
//        }
//        public ActionResult chiTietDonHang(int id)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
//                ViewBag.hinh = anhDaiDien.First().Hinh;
//            }
//            var all_ctdonhang = from tt in data.CTHoaDons where tt.MaHD == id select tt;
//            double? tongtien = 0;
//            foreach (var item in all_ctdonhang)
//            {
//                tongtien += item.ThanhTien;
//            }

//            ViewBag.Tongtien = tongtien;
//            return View(all_ctdonhang);
//        }
//        [Authorize]
//        public ActionResult xacNhanMuaNhanh(int id)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                var anhDaiDien = from s in data.NguoiDungs where s.TaiKhoan == User.Identity.Name select s;
//                ViewBag.hinh = anhDaiDien.First().Hinh;
//                ViewBag.nguoidung = anhDaiDien.First();
//            }
//            var sach = from tt in data.Saches where tt.MaSach == id select tt;         
//            return View(sach.First());
//        }

//        [Authorize]
//        public ActionResult muaNhanh(int id)
//        {
//            // Cập nhật số lượng sản phẩm trong cơ sở dữ liệu           
//            var sach = data.Saches.SingleOrDefault(s => s.MaSach == id);

//            if (sach != null)
//            {
//                sach.SoLuongSach--;
//            }       
//            data.SubmitChanges();
//            HoaDon hoadon = new HoaDon();        
//            hoadon.TaiKhoan = User.Identity.Name;
//            hoadon.NgayTao = DateTime.Now;
//            hoadon.TinhTrangThanhToan = false;
//            hoadon.TinhTrang = null;
//            data.HoaDons.InsertOnSubmit(hoadon);
//            data.SubmitChanges();          
//            CTHoaDon cthd = new CTHoaDon();
//            cthd.MaHD = hoadon.MaHD;
//            cthd.MaSach = id;
//            cthd.SoLuongMua = 1;
//            cthd.ThanhTien = sach.DonGia;
//            data.SubmitChanges();
//            data.CTHoaDons.InsertOnSubmit(cthd);
//            data.SubmitChanges();
//            var strSanPham = "";
//            double? thanhtien = 0;            
//                strSanPham += "<tr>";
//                strSanPham += "<td>" + sach.TenSach + "</td>";
//                strSanPham += "<td>" + 1 + "</td>";
//                strSanPham += "<td>" + sach.DonGia + "</td>";
//                strSanPham += "</tr>";
//                thanhtien = sach.DonGia;

//            var tongtien = thanhtien;
//            string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send2.html"));
//            contentCustomer = contentCustomer.Replace("{{MaDon}}", hoadon.MaHD.ToString());
//            contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
//            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", hoadon.NguoiDung.HoTen);
//            contentCustomer = contentCustomer.Replace("{{SDT}}", hoadon.NguoiDung.SDT);
//            contentCustomer = contentCustomer.Replace("{{Email}}", hoadon.NguoiDung.Email);
//            contentCustomer = contentCustomer.Replace("{{DiaChi}}", hoadon.NguoiDung.DiaChi);
//            contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
//            contentCustomer = contentCustomer.Replace("{{ThanhTien}}", thanhtien.ToString());
//            contentCustomer = contentCustomer.Replace("{{TongTien}}", tongtien.ToString());
//            KK_BookStore.Models.sendEmail.SendEmail("K&K BookStore", "Đơn hàng #" + hoadon.MaHD, contentCustomer.ToString(), hoadon.NguoiDung.Email);
                
//            return RedirectToAction("CamOnDatHang", "GioHang");
//        }

//        public ActionResult FailureView()
//        {
            
//            return View();
//        }
//        public ActionResult SuccessView()
//        {
//            return View();
//        }
//        public ActionResult PaymentWithPaypal(string Cancel = null)
//        {
//            //getting the apiContext  
//            APIContext apiContext = PaypalConfiguration.GetAPIContext();
//            try
//            {
//                //A resource representing a Payer that funds a payment Payment Method as paypal  
//                //Payer Id will be returned when payment proceeds or click to pay  
//                string payerId = Request.Params["PayerID"];
//                if (string.IsNullOrEmpty(payerId))
//                {
//                    //this section will be executed first because PayerID doesn't exist  
//                    //it is returned by the create function call of the payment class  
//                    // Creating a payment  
//                    // baseURL is the url on which paypal sendsback the data.  
//                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/GioHang/PaymentWithPayPal?";
//                    //here we are generating guid for storing the paymentID received in session  
//                    //which will be used in the payment execution  
//                    var guid = Convert.ToString((new Random()).Next(100000));
//                    //CreatePayment function gives us the payment approval url  
//                    //on which payer is redirected for paypal account payment  
//                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
//                    //get links returned from paypal in response to Create function call  
//                    var links = createdPayment.links.GetEnumerator();
//                    string paypalRedirectUrl = null;
//                    while (links.MoveNext())
//                    {
//                        Links lnk = links.Current;
//                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
//                        {
//                            //saving the payapalredirect URL to which user will be redirected for payment  
//                            paypalRedirectUrl = lnk.href;
//                        }
//                    }
//                    // saving the paymentID in the key guid  
//                    Session.Add(guid, createdPayment.id);
//                    return Redirect(paypalRedirectUrl);
//                }
//                else
//                {
//                    // This function exectues after receving all parameters for the payment  
//                    var guid = Request.Params["guid"];
//                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
//                    //If executed payment failed then we will show payment failure message to user  
//                    if (executedPayment.state.ToLower() != "approved")
//                    {
//                        return View("FailureView");
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                return View("FailureView");
//            }


//            List<GioHang> lstGioHang = layGioHang();

//            //Kiểm tra nếu giỏ hàng trống thì quay về trang chủ

//            data.SubmitChanges();
//            ViewBag.Tongsoluongsanpham = tongSoLuongSanPham();
//            ViewBag.Tongsoluong = tongSoLuong();
//            ViewBag.Tongtien = tongTien();
//            // Cập nhật số lượng sản phẩm trong cơ sở dữ liệu
//            foreach (var item in lstGioHang)
//            {
//                var sach = data.Saches.SingleOrDefault(s => s.MaSach == item.maSach);
//                if (sach != null)
//                {
//                    sach.SoLuongSach -= item.soLuong;
//                }
//            }
//            data.SubmitChanges();
//            HoaDon hoadon = new HoaDon();
//            List<GioHang> giohang = layGioHang();
//            hoadon.TaiKhoan = User.Identity.Name;
//            hoadon.NgayTao = DateTime.Now;
//            //hoadon.TinhTrangDonHang = false;
//            hoadon.TinhTrangThanhToan = true;
//            hoadon.TinhTrang = null;
//            data.HoaDons.InsertOnSubmit(hoadon);
//            data.SubmitChanges();
//            foreach (var item in giohang)
//            {
//                CTHoaDon cthd = new CTHoaDon();
//                cthd.MaHD = hoadon.MaHD;
//                cthd.MaSach = item.maSach;
//                cthd.SoLuongMua = item.soLuong;
//                cthd.ThanhTien = item.thanhTien;
//                data.SubmitChanges();
//                data.CTHoaDons.InsertOnSubmit(cthd);
//            }
//            data.SubmitChanges();
//            var strSanPham = "";
//            double thanhtien = 0;

//            foreach (var item in giohang)
//            {
//                strSanPham += "<tr>";
//                strSanPham += "<td>" + item.tenSach + "</td>";
//                strSanPham += "<td>" + item.soLuong + "</td>";
//                strSanPham += "<td>" + item.giaBan + "</td>";
//                strSanPham += "</tr>";
//                thanhtien += item.giaBan * item.soLuong;

//            }
//            var tongtien = thanhtien;
//            string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send2.html"));
//            contentCustomer = contentCustomer.Replace("{{MaDon}}", hoadon.MaHD.ToString());
//            contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
//            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", hoadon.NguoiDung.HoTen);
//            contentCustomer = contentCustomer.Replace("{{SDT}}", hoadon.NguoiDung.SDT);
//            contentCustomer = contentCustomer.Replace("{{Email}}", hoadon.NguoiDung.Email);
//            contentCustomer = contentCustomer.Replace("{{DiaChi}}", hoadon.NguoiDung.DiaChi);
//            contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
//            contentCustomer = contentCustomer.Replace("{{ThanhTien}}", thanhtien.ToString());
//            contentCustomer = contentCustomer.Replace("{{TongTien}}", tongtien.ToString());
//            KK_BookStore.Models.sendEmail.SendEmail("K&K BookStore", "Đơn hàng #" + 
//                hoadon.MaHD, contentCustomer.ToString(), hoadon.NguoiDung.Email);


//            var user = User.Identity.GetUserName();
//            // Xóa giỏ hàng sau khi đặt hàng thành công
//            Session[user] = null;
//            //return View(lstGioHang);
//            return RedirectToAction("CamOnDatHang", "GioHang");


//            //on successful payment, show success page to user.  
//            return View("SuccessView");
//        }
//        private PayPal.Api.Payment payment;
//        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
//        {
//            var paymentExecution = new PaymentExecution()
//            {
//                payer_id = payerId
//            };
//            this.payment = new Payment()
//            {
//                id = paymentId
//            };
//            return this.payment.Execute(apiContext, paymentExecution);
//        }
//        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
//        {
//            List<GioHang> lstGioHang = layGioHang();
//            ViewBag.Tongsoluong = tongSoLuong();
//            ViewBag.Tongtien = tongTien();
//            ViewBag.Tongsoluongsanpham = tongSoLuongSanPham();
//            //create itemlist and add item objects to it  
//            var itemList = new ItemList()
//            {
//                items = new List<Item>()
//            };
//            //Adding Item Details like name, currency, price etc
//            foreach(var item in lstGioHang)
//            {
//                itemList.items.Add(new Item()
//                {
//                    name = item.tenSach,
//                    currency = "USD",
//                    price = item.giaBan.ToString(),
//                    quantity = item.soLuong.ToString(),
//                    sku = item.maSach.ToString()
//                });
//            }
            
//            var payer = new Payer()
//            {
//                payment_method = "paypal"
//            };
//            // Configure Redirect Urls here with RedirectUrls object  
//            var redirUrls = new RedirectUrls()
//            {
//                cancel_url = redirectUrl + "&Cancel=true",
//                return_url = redirectUrl
//            };
//            // Adding Tax, shipping and Subtotal details  
//            var details = new Details()
//            {
//                tax = "0",
//                shipping = "0",
//                subtotal = tongTien().ToString()
//            };
//            //Final amount with details  
//            var amount = new Amount()
//            {
//                currency = "USD",
//                total = tongTien().ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
//                details = details
//            };
//            var transactionList = new List<Transaction>();
//            // Adding description about the transaction  
//            var paypalOrderId = DateTime.Now.Ticks;
//            transactionList.Add(new Transaction()
//            {
//                description = $"Invoice #{paypalOrderId}",
//                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
//                amount = amount,
//                item_list = itemList
//            });
//            this.payment = new Payment()
//            {
//                intent = "sale",
//                payer = payer,
//                transactions = transactionList,
//                redirect_urls = redirUrls
//            };
//            // Create a payment using a APIContext  
//            return this.payment.Create(apiContext);
//        }



//    }
//}