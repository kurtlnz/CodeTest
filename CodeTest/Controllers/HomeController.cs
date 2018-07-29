using CodeTest.Models;
using CodeTest.ViewModels;
using System;
using System.Collections.Generic;
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

        #region Student

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
        public ActionResult EditStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var student = _context.Students.SingleOrDefault(s => s.StudentId == id);

            if (student == null)
                return HttpNotFound();

            var viewModel = new StudentFormViewModel
            {
                Student = student
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

            _context.SaveChanges();

            return RedirectToAction("Index", "Home", @class.ClassId);
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

            return RedirectToAction("Index", "Home", displayClass.ClassId);
        }

        #endregion

        #region Class

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

            return RedirectToAction("Index", "Home", @class.ClassId);
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

        #endregion

        #region Index

        //public ActionResult Index()
        //{
        //    var viewModel = new IndexViewModel
        //    {
        //        Classes = _context.Classes.ToList(),
        //        Students = _context.Students.ToList()
        //    };

        //    return View(viewModel);
        //}

        public ActionResult Index(int? id)
        {
            var @class = _context.Classes.SingleOrDefault(c => c.ClassId == id);
            var viewModel = new IndexViewModel();
            
            if(@class == null)
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
                    Class = @class,
                    Classes = _context.Classes.ToList(),
                    StudentClasses = studentClasses
                };

                return View(viewModel);
            }
            
        }

        #endregion

    }

}