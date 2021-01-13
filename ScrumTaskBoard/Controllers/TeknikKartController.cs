using ScrumTaskBoard.Models;
using ScrumTaskBoard.Models.Managers;
using ScrumTaskBoard.ViewModels.Home;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ScrumTaskBoard.Controllers
{
    public class TeknikKartController : Controller
    {
        DatabaseContext db = new DatabaseContext(); // Database nesnesi üretilip, db'e bağlanılır.
        // GET: TeknikKart
        public ActionResult Yeni()
        {
            return View(); 
        }
        [HttpPost]
        ///<summary>
        /// Yeni kart eklemek için kullanılan method.
        ///</summary>
        ///<param name="kart"></param>
        public ActionResult Yeni(TeknikKart kart)
        {
            db.TeknikKart.Add(kart); // kart parametresi TeknikKart db'ine eklenir.

            int sonuc = db.SaveChanges(); // db kayıt edilir ve sonuc değişkenine atılır.

            #region Db kayıt etme sonucuna göre sayfaya alert gönderilir.
            if (sonuc > 0)
            {
                ViewBag.Result = "Kart ekleme işlemi gerçekleşmiştir.";
                ViewBag.Status = "success";
            }
            else
            {
                ViewBag.result = "Kart ekleme işleminde hata oluştu.";
                ViewBag.Status = "danger";
            }
            #endregion

            return View();
        }
        [HttpGet]
        ///<summary>
        /// Düzenlenicek kart'ın kartId'si alınarak view'a gönderilen method.
        ///</summary>
        ///<param name="kartId"></param>
        public ActionResult Duzenle(int? kartId) // Düzenlenicek kart'ın kartId parametresi alınır.
        {
            TeknikKart kart = new TeknikKart(); // TeknikKart sınıfından bir adet nesne oluşturulur.
            if (kartId != null) // kartId isimli parametre boş değilse,
            {
                kart = db.TeknikKart.Where(x => x.teknikKart_Id == kartId).FirstOrDefault(); // db'de olan kart ile kullanıcının güncellemek istediği kart ıd'leri eşleşiyor ise, eşleşen kart satırı alınır.
                kart.Is = new List<Is>(); // kart'ın işlerinini tutulması için listte tutulur.
                kart.Is = db.Is.Where(x => x.teknikKart_Id == kartId).ToList(); // eşleşen kart'ın işi kart.Is nesnesine aktarılır.
            }
            return View(kart); // View'a kart nesnesi gönderilir.
        }
        [HttpPost]
        ///<summary>
        /// View'a gönderilen kartın düzenlenmesi için yazılan method.
        ///</summary>
        ///<param name="kart"></param>
        public ActionResult Duzenle(TeknikKart kart) // Güncellenicek kart nesnesi parametre olarak alınır.
        {
            TeknikKart kartGuncelle = db.TeknikKart.Where(x => x.teknikKart_Id == kart.teknikKart_Id).FirstOrDefault(); // db'de böyle bir kartın olup olmadığı kontrol edilir.
            int kartID = kart.teknikKart_Id; // teknikKart_Id değişkeni view'da disabled olduğundan dolayı tekrar bir değişkene atılır.
            if (kartGuncelle != null) // db'de kart var ise,
            {
                if (kartGuncelle.Is != null) // kart'ın işi var ise,
                {
                    kartGuncelle.Is = kart.Is;
                }
                else // kart'ın yok ise,
                    kartGuncelle.Is = null; 
                #region Kart bilgileri güncellenir.
                kartGuncelle.isinAciklamasi = kart.isinAciklamasi;
                kartGuncelle.notlar = kart.notlar;
                kartGuncelle.projeAdi = kart.projeAdi;
                kartGuncelle.tahminiSure = kart.tahminiSure;
                kartGuncelle.tarih = kart.tarih;
                kartGuncelle.teknikUzmanAdı = kart.teknikUzmanAdı;
                #endregion

                int sonuc = db.SaveChanges(); // Güncellenen kart db'e kayıt edilir.

                #region Db kayıt etme sonucuna göre sayfaya alert gönderilir.
                if (sonuc > 0)
                {
                    ViewBag.Result = "Kart güncelleme işlemi gerçekleşmiştir.";
                    ViewBag.Status = "success";
                }
                else
                {
                    ViewBag.result = "Kart güncelleme işleminde hata oluştu.";
                    ViewBag.Status = "danger";
                }
                #endregion
                kart = new TeknikKart();
                db = new DatabaseContext();
                kart = db.TeknikKart.Where(x => x.teknikKart_Id == kartID).FirstOrDefault();
                kart.Is = new List<Is>();
                kart.Is = db.Is.Where(x => x.teknikKart_Id == kartID).ToList();
            }

            return View(kart);
        }
        [HttpGet]
        ///<summary>
        /// Teknik kart silmek için silinecek olan kart'ın bilgilerini view'a göndermek için yazılan method.
        ///</summary>
        ///<param name="teknikKartId"></param>
        public ActionResult Sil(int? teknikKartId)
        {
            TeknikKart silinecekKart = null;
            if (teknikKartId != null)
            {
                silinecekKart = db.TeknikKart.Where(x => x.teknikKart_Id == teknikKartId).FirstOrDefault();
            }
            return View(silinecekKart);
        }
        ///<summary>
        /// View'a gönderilen kartın silinme işlemi için yazılan method.
        ///</summary>
        ///<param name="teknikKartId"></param>
        [HttpPost, ActionName("Sil")]
        public ActionResult SilOnay(int? teknikKartId)
        {
            if (teknikKartId != null)
            {
                TeknikKart silinecekKart = db.TeknikKart.Where(x => x.teknikKart_Id == teknikKartId).FirstOrDefault();

                db.TeknikKart.Remove(silinecekKart);

                ViewBag.Result = "Silme işlemi başarılır.";
                ViewBag.Status = "info";

                db.SaveChanges();
            }
            Thread.Sleep(1000);
            // Home/HomePage sayfasına gider.
            return RedirectToAction("HomePage", "Home");
        }
        ///<summary>
        /// Kart düzenlenirken, karta ait işlerin sürükle bırak yöntemi ile sürüklendikten sonra db'de güncellenmesi için kullanılan method.
        ///</summary>
        ///<param name="islerim"></param>
        [HttpPost]
        public int IsDuzenle(Is islerim)
        {
            Is updateIs = db.Is.Where(x=>x.isTakip_Id == islerim.isTakip_Id).FirstOrDefault();
            int result = 0;
            if (updateIs != null)
            {
                updateIs.durum = islerim.durum;

                int sonuc = db.SaveChanges();

                if (sonuc > 0)
                {
                    result = 1;
                }               
            }
            return result;
        }
    }
}