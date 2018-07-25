using CodeTest.Models;
using CodeTest.ViewModels;
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

        public ActionResult New()
        {
            return View("ClassForm");
        }

        [HttpPost]
        public ActionResult SaveClass(Class @class)
        {
            if (@class.ClassId == 0)
                _context.Classes.Add(@class);
            else
            {
                var classInDb = _context.Classes.Single(c => c.ClassId == @class.ClassId);

                classInDb.ClassName = @class.ClassName;
                classInDb.Location = @class.Location;
                classInDb.TeacherName = @class.TeacherName;
            }
            
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult EditClass(int id)
        {
            var @class = _context.Classes.SingleOrDefault(c => c.ClassId == id);

            if (@class == null)
                return HttpNotFound();

            return View("ClassForm", @class);
        }

        public ActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                Classes = _context.Classes.ToList(),
                Students = _context.Students.ToList(),
                StudentClasses = _context.StudentClasses.ToList()
            };

            return View(viewModel);
        }

    }

}