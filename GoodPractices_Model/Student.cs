using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GoodPractices_Model
{
    public class Student : Person
    {
        public List<Grade> Grades { get; set; }
        public ForeignLanguage ForeignLanguaje { set; get; }


        public Student(string document, string name, int age)
        {
            this.Document = document;
            this.Age = age;
            this.Name = name;
            this.Grades = new List<Grade>();
        }

        public Student()
        {
            this.Grades = new List<Grade>();
        }
    }
}
