using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBookStoreAn.Models;
using PagedList;
using PagedList.Mvc;

namespace MVCBookStoreAn.Controllers
{
    public class BookStoreController : Controller
    {
        //
        // GET: /BookStore/
        dbBookManagerDataContext data = new dbBookManagerDataContext();
        private List<SACH> Laysachmoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Index(int ? page, string searchString)
        {
            var sp = from e in data.SACHes select e;
            if (!String.IsNullOrEmpty(searchString))
            {
                sp = sp.Where(s => s.Tensach.Contains(searchString));
            }
            ViewBag.SeachString = searchString;
            int pageSize = 6;
            int pageNum = (page ?? 1);
            //var sachmoi = Laysachmoi(15);
            return View(sp.ToList().OrderBy(n => n.Masach).ToPagedList(pageNum, pageSize));
        }
        public ActionResult ChuDe()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult Nhaxuatban()
        {
            var NXB = from nxb in data.NHAXUATBANs select nxb;
            return PartialView(NXB);
        }
        public ActionResult SPTheochude(int id)
        {
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return View(sach);
        }
        public ActionResult SPTheoNXB(int id)
        {
            var sach = from s in data.SACHes where s.MaNXB == id select s;
            return View(sach);
        }
        public ActionResult Details(int id)
        {
            var sach = from s in data.SACHes where s.Masach == id select s;
            return View(sach.Single());
        }
    }
}
