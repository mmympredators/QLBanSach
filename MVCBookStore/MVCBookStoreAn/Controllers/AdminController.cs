using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBookStoreAn.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Data.Entity;

namespace MVCBookStoreAn.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        dbBookManagerDataContext db = new dbBookManagerDataContext();
        //QLBANSACHEntities1 db = new QLBANSACHEntities1();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sach(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.SACHes.ToList().OrderBy(n => n.Masach).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        //Thêm mới sách
        [HttpGet]
        public ActionResult Themmoisach()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoisach(SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            //Kiểm tra đường dẫn file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    // Lưu tên file, lưu ý bổ sung thư viện using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    // Lưu đường dẫn file
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    // Kiễm tra hình ảnh tồn tại
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        //Lưu hình ảnh vào đường dẫn
                        fileUpload.SaveAs(path);
                    }
                    sach.Hinhminhhoa = fileName;
                    // Lưu vào CSDL
                    db.SACHes.InsertOnSubmit(sach);
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("Sach");
        }

        //Hiển thị sản phẩm
        public ActionResult Chitietsach(int id)
        {
            //Lấy ra đối tượng sách theo mã
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        //Xóa sản phẩm
        [HttpGet]
        public ActionResult Xoasach(int id)
        {
            //Lấy ra đối tượng sách cần xóa theo mã
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpPost, ActionName("Xoasach")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lấy ra đối tượng sách cần xóa theo mã
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SACHes.DeleteOnSubmit(sach);
            db.SubmitChanges();
            return RedirectToAction("Sach");
        }

        //Chỉnh sửa sản phẩm
        [HttpGet]
        public ActionResult Suasach(int id)
        {
            //Lấy ra đối tượng sách cần sửa theo mã
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasach(SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            var s = db.SACHes.SingleOrDefault(n => n.Masach == sach.Masach);
            //Kiểm tra đường dẫn file
            //if (fileUpload == null)
            //{
            //    s.Tensach = sach.Tensach;
            //    s.Mota = sach.Mota;
            //    s.Dongia = sach.Dongia;
            //    s.MaCD = sach.MaCD;
            //    s.MaNXB = sach.MaNXB;
            //    s.Ngaycapnhat = sach.Ngaycapnhat;
            //    s.Soluongban = sach.Soluongban;
            //    db.SubmitChanges();
            //    return View(sach);
            //}
            //Thêm vào CSDL
            //else
            //{
            if (ModelState.IsValid)
            {
                //Lưu tên file, lưu ý bổ sung thư viên System.IO;
                //var fileName = Path.GetFileName(fileUpload.FileName);
                ////Lưu đường dẫn của file
                //var path = Path.Combine(Server.MapPath("~/images"), fileName);
                //// Kiễm tra hình ảnh tồn tại
                //if (System.IO.File.Exists(path))
                //{
                //    ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                //}
                //else
                //{
                //    //Lưu hình ảnh vào đường dẫn
                //    fileUpload.SaveAs(path);
                //}
                //sach.Hinhminhhoa = fileName;
                // Lưu vào CSDL
                //var s = db.SACHes.SingleOrDefault(n => n.Masach == sach.Masach);
                s.Tensach = sach.Tensach;
                s.Mota = sach.Mota;
                s.Dongia = sach.Dongia;
                s.MaCD = sach.MaCD;
                s.MaNXB = sach.MaNXB;
                s.Ngaycapnhat = sach.Ngaycapnhat;
                //db.SACHes.InsertOnSubmit(s);
                db.SubmitChanges();
            }
            return RedirectToAction("Sach");
            //}
        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Suasach(SACH sach, HttpPostedFileBase fileUpload)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //Thực hiện trực tiếp cập nhật vào Model
        //        db.Entry(sach).State = System.Data.Entity.EntityState.Modified;
        //        db.SubmitChanges();
        //    }
        //    return RedirectToAction("Sach");
        //    //}
        //}
    }
}
