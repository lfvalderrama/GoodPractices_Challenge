﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodPractices_Model
{
    public class Teacher : Person
    {
        public Course Course { get; set; }
        public List<Subject> Subjects { get; set; }

        public Teacher(Course course, List<Subject> subjects, String name, string cc, int age)
        {
            this.Course = course;
            this.Subjects = subjects;
            this.Age = age;
            this.Document = cc;
            this.Name = name;
        }
    }
}
