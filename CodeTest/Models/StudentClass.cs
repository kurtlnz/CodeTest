using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeTest.Models
{
    public class StudentClass
    {
        public int StudentClassId { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }

        public virtual Student Student { get; set; }
        public virtual Class Class { get; set; }
    }
}