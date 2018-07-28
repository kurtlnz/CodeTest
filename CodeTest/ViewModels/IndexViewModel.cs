using CodeTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeTest.ViewModels
{
    public class IndexViewModel
    {
        public Class Class { get; set; }
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Class> Classes { get; set; }
        public IEnumerable<StudentClass> StudentClasses { get; set; }
        
    }
}