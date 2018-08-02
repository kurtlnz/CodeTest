using CodeTest.Models;
using CodeTest.ViewModels;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace CodeTest.Controllers
{
    public class ClassController : Controller
    {
        private DbContextModel _context;

        public ClassController()
        {
            _context = new DbContextModel();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult NewClass()
        {
            return View("ClassForm");
        }

        // GET
        public ActionResult EditClass(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var @class = _context.Classes.SingleOrDefault(c => c.ClassId == id);

            if (@class == null)
                return HttpNotFound();

            return View("ClassForm", @class);
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

            var displayClass = @class;

            //return RedirectToAction("Index", "Home", new { id = displayClass.ClassId });
            return new JsonResult();
        }

        // GET
        public ActionResult DeleteClass(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var @class = _context.Classes.SingleOrDefault(c => c.ClassId == id);

            if (@class == null)
                return HttpNotFound();

            var viewModel = new DeleteFormViewModel
            {
                Class = @class
            };

            return View("DeleteForm", viewModel);
        }

        [HttpPost]
        public ActionResult DeleteClassConfirm(Class @class)
        {
            var classInDb = _context.Classes.SingleOrDefault(c => c.ClassId == @class.ClassId);
            var studentClassesInDb = _context.StudentClasses.Include(s => s.Student).Where(sc => sc.ClassId == @class.ClassId).ToList();
            var studentsInDb = studentClassesInDb.Select(s => s.Student).ToList();

            if (classInDb != null)
            {
                _context.Classes.Remove(classInDb);
                _context.Students.RemoveRange(studentsInDb);
                _context.StudentClasses.RemoveRange(studentClassesInDb);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}