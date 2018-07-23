using System.ComponentModel.DataAnnotations;

namespace CodeTest.Models
{
    public class Class
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string Location { get; set; }
        public string TeacherName { get; set; }
    }
}