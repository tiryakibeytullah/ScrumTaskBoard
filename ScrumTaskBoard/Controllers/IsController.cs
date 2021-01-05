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
        // GET: Is
        public ActionResult Yeni()
        {
            DatabaseContext db = new DatabaseContext();

            List<SelectListItem> kartList =
                (from kart in db.TeknikKart.ToList()
                 select new SelectListItem()
                 {
                     Text = kart.teknikKart_Id + " - " + kart.projeAdi + " / " + kart.teknikUzmanAdı,
                     Value = kart.teknikKart_Id.ToString()
                 }).ToList();

            TempData["kartlar"] = kartList;
            ViewBag.kartlar = kartList;

            List<SelectListItem> durumList = (from durum in db.Durum.ToList()
                                              select new SelectListItem()
                                              {
                                                  Text = durum.DurumBilgisi,
                                                  Value = durum.Id.ToString()
                                              }).ToList();
            TempData["durumlar"] = durumList;
            ViewBag.durumlar = durumList;

            return View();
        }
        [HttpPost]
        public ActionResult Yeni(Is yeniIs)
        {
            DatabaseContext db = new DatabaseContext();
            TeknikKart kart = db.TeknikKart.Where(x => x.teknikKart_Id == yeniIs.teknikKart_Id).FirstOrDefault();

            if (kart != null)
            {
                yeniIs.teknikKart_Id = kart.teknikKart_Id;
                db.Is.Add(yeniIs);

                int sonuc = db.SaveChanges();

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
            }

            ViewBag.kartlar = TempData["kartlar"];
            ViewBag.durumlar = TempData["durumlar"];

            return View();
        }
        public ActionResult Duzenle(int? isId)
        {
            Is guncellenicekIs = new Is();
            DatabaseContext db = new DatabaseContext();
            if (isId != null)
            {
                guncellenicekIs = db.Is.Where(x => x.isTakip_Id == isId).FirstOrDefault();
            }

            List<SelectListItem> durumList = (from durum in db.Durum.ToList()
                                              select new SelectListItem()
                                              {
                                                  Text = durum.DurumBilgisi,
                                                  Value = durum.Id.ToString()
                                              }).ToList();
            TempData["durumlar"] = durumList;
            ViewBag.durumlar = durumList;

            return View(guncellenicekIs);
        }
        [HttpPost]
        public ActionResult Duzenle(Is Is, int? isId)
        {
            DatabaseContext db = new DatabaseContext();
            Is guncellenicekIs = db.Is.Where(x => x.isTakip_Id == isId).FirstOrDefault();

            if (guncellenicekIs != null)
            {
                guncellenicekIs.tarih = Is.tarih;
                guncellenicekIs.durum = Is.durum;
                guncellenicekIs.yapilacak_Is = Is.yapilacak_Is;
                guncellenicekIs.aciklama = Is.aciklama;

                int sonuc = db.SaveChanges();

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
            }

            ViewBag.durumlar = TempData["durumlar"];

            return View(guncellenicekIs);
        }
        [HttpGet]
        public ActionResult Sil(int? isId)
        {
            Is silinecekIs = null;
            if (isId != null)
            {
                DatabaseContext db = new DatabaseContext();
                silinecekIs = db.Is.Where(x => x.isTakip_Id == isId).FirstOrDefault();
            }
            return View(silinecekIs);
        }
        [HttpPost, ActionName("Sil")] // TeknikKart/Sil
        public ActionResult SilOnay(int? isId)
        {
            if (isId != null)
            {
                DatabaseContext db = new DatabaseContext();
                Is silinecekIs = db.Is.Where(x => x.isTakip_Id == isId).FirstOrDefault();

                db.Is.Remove(silinecekIs);

                ViewBag.Result = "Silme işlemi başarılır.";
                ViewBag.Status = "info";

                db.SaveChanges();
            }
            Thread.Sleep(1000);
            // Home/HomePage sayfasına gider.
            return RedirectToAction("HomePage", "Home");
        }
    }
}