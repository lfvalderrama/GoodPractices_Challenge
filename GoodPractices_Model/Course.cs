using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodPractices_Model
{
    public class Course
    {
        public int Id { get; set; }
        public List<Student> Students { get; set; }
        public List<Subject> Subjects { get; set; }
        public Student Headman { get; set; }

        public Course(List<Student> students, List<Subject> subjects, Student headman)
        {
            this.Students = students;
            this.Subjects = subjects;
            this.Headman = headman;
        }
    }
}
