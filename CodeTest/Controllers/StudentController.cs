using CodeTest.Models;
using CodeTest.ViewModels;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CodeTest.Controllers
{
    public class StudentController : Controller
    {
        private DbContextModel _context;

        public StudentController()
        {
            _context = new DbContextModel();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult NewStudent(int? classId)
        {
            if (classId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var @class = _context.Classes.SingleOrDefault(s => s.ClassId == classId);

            if (@class == null)
                return HttpNotFound();

            var viewModel = new StudentFormViewModel
            {
                Class = @class
            };

            return View("StudentForm", viewModel);
        }

        // GET
        public ActionResult EditStudent(int? studentId, int? classId)
        {
            if (studentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var student = _context.Students.SingleOrDefault(s => s.StudentId == studentId);

            if (student == null)
                return HttpNotFound();

            var displayClass = _context.Classes.SingleOrDefault(c => c.ClassId == classId);

            var viewModel = new StudentFormViewModel
            {
                Student = student,
                Class = displayClass
            };

            return View("StudentForm", viewModel);
        }

        [HttpPost]
        public ActionResult SaveStudent(StudentFormViewModel viewModel)
        {
            var student = viewModel.Student;
            var @class = viewModel.Class;
            var studentClass = new StudentClass { ClassId = @class.ClassId, StudentId = student.StudentId };

            if (student.StudentId == 0)
            {
                _context.Students.Add(student);
                _context.StudentClasses.Add(studentClass);
            }
            else
            {
                var studentInDb = _context.Students.Single(c => c.StudentId == student.StudentId);

                studentInDb.GivenName = student.GivenName;
                studentInDb.Surname = student.Surname;
                studentInDb.AgeInYears = student.AgeInYears;
                studentInDb.GPA = student.GPA;
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                var error = ex.GetBaseException().Message;

                if (error.Contains("Cannot insert duplicate key row in object 'dbo.Students' with unique index 'SurnameIndex'"))
                    return View("ErrorDuplicateKey");

                throw;
            }

            var displayClass = @class;

            return RedirectToAction("Index", "Home", new { id = displayClass.ClassId });
        }

        // GET
        public ActionResult DeleteStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var student = _context.Students.SingleOrDefault(s => s.StudentId == id);

            if (student == null)
                return HttpNotFound();

            var viewModel = new DeleteFormViewModel
            {
                Student = student
            };

            return View("DeleteForm", viewModel);
        }

        [HttpPost]
        public ActionResult DeleteStudentConfirm(DeleteFormViewModel viewModel)
        {
            var student = viewModel.Student;

            var studentInDb = _context.Students.SingleOrDefault(s => s.StudentId == student.StudentId);
            var studentClassInDb = _context.StudentClasses.SingleOrDefault(sc => sc.StudentId == student.StudentId);

            if (studentInDb != null)
            {
                _context.Students.Remove(studentInDb);
                _context.StudentClasses.Remove(studentClassInDb);
                _context.SaveChanges();
            }

            var displayClass = _context.Classes.SingleOrDefault(c => c.ClassId == studentClassInDb.ClassId);

            return RedirectToAction("Index", "Home", new { id = displayClass.ClassId });
        }
    }
}