using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FI.WebAtividadeEntrevista.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Responsavel pela View Index
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Responsavel pela View About
        /// </summary>
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        /// <summary>
        /// Responsavel pela View Contact
        /// </summary>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}