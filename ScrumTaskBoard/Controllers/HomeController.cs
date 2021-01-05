﻿using ScrumTaskBoard.Models;
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
        public ActionResult HomePage()
        {
            DatabaseContext db = new DatabaseContext();

            HomePageViewModel model = new HomePageViewModel();
            model.TeknikKart = db.TeknikKart.ToList();
            model.Is = db.Is.ToList();
            model.Durum = db.Durum.ToList();

            return View(model);
        }
    }
}