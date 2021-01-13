using ScrumTaskBoard.Models;
using ScrumTaskBoard.Models.Managers;
using ScrumTaskBoard.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScrumTaskBoard.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        ///<summary>
        /// HomePage'e gönderilen modeller.
        ///</summary>
        public ActionResult HomePage()
        {
            #region HomePage view'ına gönderilen modeller.
            DatabaseContext db = new DatabaseContext(); //Database context sınıfına bağlayıp db bağlatısı kuruluyor.

            HomePageViewModel model = new HomePageViewModel(); //Modelin içerisine db'de olan tablolar aktarılıyor.
            model.TeknikKart = db.TeknikKart.ToList();
            model.Is = db.Is.ToList();
            model.Durum = db.Durum.ToList();

            return View(model); // Model view'a gönderiliyor.
            #endregion
        }
    }
}