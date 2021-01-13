using ScrumTaskBoard.Models;
using ScrumTaskBoard.Models.Managers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ScrumTaskBoard.Controllers
{
    public class IsController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        // GET: Is
        ///<summary>
        /// İş eklemek için view'a projede olan kartları gönderen method.
        ///</summary>
        public ActionResult Yeni()
        {
            #region TeknikKartları List
            List<SelectListItem> kartList =
                (from kart in db.TeknikKart.ToList()
                 select new SelectListItem()
                 {
                     Text = kart.teknikKart_Id + " - " + kart.projeAdi + " / " + kart.teknikUzmanAdı,
                     Value = kart.teknikKart_Id.ToString()
                 }).ToList();

            TempData["kartlar"] = kartList;
            ViewBag.kartlar = kartList;
            #endregion

            #region Database'de olan durumları tutan List
            List<SelectListItem> durumList = (from durum in db.Durum.ToList()
                                              select new SelectListItem()
                                              {
                                                  Text = durum.DurumBilgisi,
                                                  Value = durum.Id.ToString()
                                              }).ToList();
            TempData["durumlar"] = durumList;
            ViewBag.durumlar = durumList;
            #endregion

            return View();
        }
        [HttpPost]
        ///<summary>
        /// View'a gönderilen işin eklenmesi için yazılan method.
        ///</summary>
        ///<param name="yeniIs"></param>
        public ActionResult Yeni(Is yeniIs)
        {
            TeknikKart kart = db.TeknikKart.Where(x => x.teknikKart_Id == yeniIs.teknikKart_Id).FirstOrDefault(); // Seçilen teknik kart ile iş teknik kart'ı birbirine eşit olan kart bulunur. Kısaca hangi karta iş eklemek istediğimiz seçeriz 

            if (kart != null)
            {
                yeniIs.teknikKart_Id = kart.teknikKart_Id; 
                db.Is.Add(yeniIs); // Yeni iş db'e eklenir.

                kart.tahminiSure += 10;
                int sonuc = db.SaveChanges(); // Yeni iş db'e kayıt edilir.

                

                #region Db kayıt etme sonucuna göre sayfaya alert gönderilir.
                if (sonuc > 0)
                {
                    ViewBag.Result = "Yeni iş takip planı kaydedilmiştir.";
                    ViewBag.Status = "success";
                }
                else
                {
                    ViewBag.Result = "Yeni iş takip planı kaydedilememiştir.";
                    ViewBag.Status = "danger";
                }
                #endregion
            }

            ViewBag.kartlar = TempData["kartlar"];
            ViewBag.durumlar = TempData["durumlar"];

            return View();
        }
        ///<summary>
        /// Düzenlenicek iş'in view'a gönderilmesi methodu.
        ///</summary>
        ///<param name="isId"></param>
        public ActionResult Duzenle(int? isId)
        {
            Is guncellenicekIs = new Is();
            if (isId != null)
            {
                guncellenicekIs = db.Is.Where(x => x.isTakip_Id == isId).FirstOrDefault(); // isId null değil ise, db'de bulunan iş id eşleştirilip guncellenicek iş satırı alınır.
            }

            #region Db durum bilgileri List'i
            List<SelectListItem> durumList = (from durum in db.Durum.ToList()
                                              select new SelectListItem()
                                              {
                                                  Text = durum.DurumBilgisi,
                                                  Value = durum.Id.ToString()
                                              }).ToList();
            TempData["durumlar"] = durumList;
            ViewBag.durumlar = durumList;
            #endregion

            return View(guncellenicekIs); // Guncellenicek iş view'a gönderilir.
        }
        [HttpPost]
        ///<summary>
        /// View'a gönderilen işin düzenlenmesi için yazılan method.
        ///</summary>
        ///<param name="Is"></param>
        ///<param name="isId"></param>
        public ActionResult Duzenle(Is Is, int? isId)
        {
            Is guncellenicekIs = db.Is.Where(x => x.isTakip_Id == isId).FirstOrDefault(); // güncellenicek iş db'de var mı yok mu konrtolu yapılır.

            if (guncellenicekIs != null) // guncellenicek iş db'de var ise, güncelleme işlemi başlar.
            {
                guncellenicekIs.tarih = Is.tarih;
                guncellenicekIs.durum = Is.durum;
                guncellenicekIs.yapilacak_Is = Is.yapilacak_Is;
                guncellenicekIs.aciklama = Is.aciklama;

                int sonuc = db.SaveChanges();
                #region Db kayıt etme sonucuna göre sayfaya alert gönderilir.
                if (sonuc > 0)
                {
                    ViewBag.Result = "İş güncelleme işlemi gerçekleşmiştir.";
                    ViewBag.Status = "success";
                }
                else
                {
                    ViewBag.result = "İş güncelleme işleminde hata oluştu.";
                    ViewBag.Status = "danger";
                }
                #endregion
            }

            ViewBag.durumlar = TempData["durumlar"];

            return View(guncellenicekIs);
        }
        [HttpGet]
        ///<summary>
        /// Silinecek iş'in view'da görüntülenmesi methodu.
        ///</summary>
        ///<param name="isId"></param>
        public ActionResult Sil(int? isId)
        {
            Is silinecekIs = null;
            if (isId != null)
            {
                silinecekIs = db.Is.Where(x => x.isTakip_Id == isId).FirstOrDefault(); // Silinecek iş db'de var ise view'a gönderilir.
            }
            return View(silinecekIs); // View'a gönderilir.
        }
        [HttpPost, ActionName("Sil")] // TeknikKart/Sil
        ///<summary>
        /// View'a gönderilen işin silinmesi işlemi.
        ///</summary>
        ///<param name="isId"></param>
        public ActionResult SilOnay(int? isId)
        {
            if (isId != null)
            {
                Is silinecekIs = db.Is.Where(x => x.isTakip_Id == isId).FirstOrDefault(); // silinecek iş db'de var ise eşleştirilir.
                TeknikKart kart = db.TeknikKart.Where(x => x.teknikKart_Id == silinecekIs.teknikKart_Id).FirstOrDefault();

                kart.tahminiSure -= 10;

                db.Is.Remove(silinecekIs); // Db'den iş silinir.

                #region İş silindikten sonra alert basılır ve Db'e kayıt edilir.
                ViewBag.Result = "Silme işlemi başarılır.";
                ViewBag.Status = "info";

                db.SaveChanges();
                #endregion
            }
            Thread.Sleep(1000);
            // Home/HomePage sayfasına gider.
            return RedirectToAction("HomePage", "Home");
        }
    }
}