using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeTest.Models
{
    public class Class
    {
        public int ClassId { get; set; }
        [Display(Name = "Class Name")]
        public string ClassName { get; set; }
        public string Location { get; set; }
        [Display(Name = "Teacher Name")]
        public string TeacherName { get; set; }

        public virtual ICollection<StudentClass> StudentClasses { get; set; }

    }
}