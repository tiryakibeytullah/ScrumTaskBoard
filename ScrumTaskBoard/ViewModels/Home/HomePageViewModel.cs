using ScrumTaskBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumTaskBoard.ViewModels.Home
{
    public class HomePageViewModel
    {
        public List<TeknikKart> TeknikKart { get; set; }
        public List<Is> Is { get; set; }
        public List<Durum> Durum { get; set; }
    }
}