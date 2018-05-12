using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBookStoreAn.Models
{
    public class Giohang
    {
        dbBookManagerDataContext data = new dbBookManagerDataContext();
        public int iMasach { get; set; }
        public string sTensach { get; set; }
        public string sAnhbia { get; set; }
        public Double dDongia { get; set; }
        public int iSoluong { get; set; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        // Khởi tạo giỏ hàng theo mã sách được truyền vào với số lượng
        public Giohang(int Masach)
        {
            iMasach = Masach;
            SACH sach = data.SACHes.Single(n => n.Masach == iMasach);
            sTensach = sach.Tensach;
            sAnhbia = sach.Hinhminhhoa;
            dDongia = double.Parse(sach.Dongia.ToString());
            iSoluong = 1;
        }
    }
}