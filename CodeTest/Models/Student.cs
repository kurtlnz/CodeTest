using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeTest.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string GivenName { get; set; }
        [Index("SurnameIndex", IsUnique = true)]
        [MaxLength(255)]
        public string Surname { get; set; }
        public int AgeInYears { get; set; }
        public double GPA { get; set; }
    }
}