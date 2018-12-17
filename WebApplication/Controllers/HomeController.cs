using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = I18N.WebApplication.About_Subtitle;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message =  I18N.WebApplication.Contact_Subtitle;

            return View();
        }
    }
}