using CodeTest.Models;
using CodeTest.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CodeTest.Controllers
{
    public class HomeController : Controller
    {
        private DbContextModel _context;

        public HomeController()
        {
            _context = new DbContextModel();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                Classes = _context.Classes.ToList()
            };

            return View(viewModel);
        }

    }

}