using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
        public String Name { get; set; }

        public Course()
        {
        }

        public Course(String name, List<Student> students, List<Subject> subjects, Student headman)
        {
            this.Students = students;
            this.Subjects = subjects;
            this.Headman = headman;
            this.Name = name;
        }

        public Course(String name, Student headman)
        {
            this.Students = new List<Student>();
            this.Subjects = new List<Subject>();
            this.Headman = headman;
            this.Name = name;
        }
    }
}
