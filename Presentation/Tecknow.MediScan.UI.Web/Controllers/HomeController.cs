using System.Web.Mvc;
using Tecknow.MediScan.Business.RepositoryPattern.Repository;
using Tecknow.MediScan.Business.UnitOfWork;
using Tecknow.MediScan.Entities;

namespace Tecknow.MediScan.UI.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
          
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}