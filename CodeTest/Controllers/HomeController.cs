using CodeTest.Models;
using CodeTest.ViewModels;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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

        public ActionResult Index(int? id)
        {
            var displayClass = _context.Classes.SingleOrDefault(c => c.ClassId == id);
            var viewModel = new IndexViewModel();
            
            // If there is no class to display
            if(displayClass == null)
            {
                viewModel = new IndexViewModel
                {
                    Classes = _context.Classes.ToList(),
                    StudentClasses = null
                };

                return View(viewModel);
            }
            else
            {
                var studentClasses = _context.StudentClasses.Include(s => s.Student).Where(c => c.ClassId == id).ToList();

                viewModel = new IndexViewModel
                {
                    Class = displayClass,
                    Classes = _context.Classes.ToList(),
                    StudentClasses = studentClasses
                };

                return View(viewModel);
            }
            
        }

    }

}