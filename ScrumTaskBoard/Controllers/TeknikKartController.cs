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
        // GET: TeknikKart
        public ActionResult Yeni()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Yeni(TeknikKart kart)
        {
            DatabaseContext db = new DatabaseContext();
            db.TeknikKart.Add(kart);

            int sonuc = db.SaveChanges();

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

            return View();
        }
        [HttpGet]
        public ActionResult Duzenle(int? kartId)
        {
            TeknikKart kart = new TeknikKart();
            if (kartId != null)
            {
                DatabaseContext db = new DatabaseContext();
                kart = db.TeknikKart.Where(x => x.teknikKart_Id == kartId).FirstOrDefault();
                kart.Is = new List<Is>();
                kart.Is = db.Is.Where(x => x.teknikKart_Id == kartId).ToList();
            }
            return View(kart);
        }
        [HttpPost]
        public ActionResult Duzenle(TeknikKart kart)
        {
            DatabaseContext db = new DatabaseContext();
            TeknikKart kartGuncelle = db.TeknikKart.Where(x => x.teknikKart_Id == kart.teknikKart_Id).FirstOrDefault();
            int kartID = kart.teknikKart_Id;
            if (kartGuncelle != null)
            {
                if (kartGuncelle.Is != null)
                {
                    kartGuncelle.Is = kart.Is;
                }
                else
                    kartGuncelle.Is = null;
                //kartGuncelle.Is = (kartGuncelle.Is != null) ? kart.Is : null;
                kartGuncelle.isinAciklamasi = kart.isinAciklamasi;
                kartGuncelle.notlar = kart.notlar;
                kartGuncelle.projeAdi = kart.projeAdi;
                kartGuncelle.tahminiSure = kart.tahminiSure;
                kartGuncelle.tarih = kart.tarih;
                kartGuncelle.teknikUzmanAdı = kart.teknikUzmanAdı;

                int sonuc = db.SaveChanges();

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
                kart = new TeknikKart();
                db = new DatabaseContext();
                kart = db.TeknikKart.Where(x => x.teknikKart_Id == kartID).FirstOrDefault();
                kart.Is = new List<Is>();
                kart.Is = db.Is.Where(x => x.teknikKart_Id == kartID).ToList();
            }

            return View(kart);
        }
        public ActionResult Sil(int? teknikKartId)
        {
            TeknikKart silinecekKart = null;
            if (teknikKartId != null)
            {
                DatabaseContext db = new DatabaseContext();
                silinecekKart = db.TeknikKart.Where(x => x.teknikKart_Id == teknikKartId).FirstOrDefault();
            }
            return View(silinecekKart);
        }
        [HttpPost, ActionName("Sil")]
        public ActionResult SilOnay(int? teknikKartId)
        {
            if (teknikKartId != null)
            {
                DatabaseContext db = new DatabaseContext();
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
        [HttpPost]
        public int IsDuzenle(Is islerim)
        {
            DatabaseContext db = new DatabaseContext();
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