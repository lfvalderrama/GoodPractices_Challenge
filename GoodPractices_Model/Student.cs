using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GoodPractices_Model
{
    public class Student : Person
    {
        public Dictionary<Subject, List<Grade>> PartialGrades { get; }
        public Dictionary<Subject, List<Grade>> FinalGrades { get; }
        public ForeignLanguage ForeignLanguaje { set; get; }


        public Student(string document, string name, int age, ForeignLanguage foreignLanguaje)
        {
            this.Document = document;
            this.Age = age;
            this.Name = name;
            this.PartialGrades = new Dictionary<Subject, List<Grade>>();
            this.FinalGrades = new Dictionary<Subject, List<Grade>>();
            this.ForeignLanguaje = foreignLanguaje;
        }

        public Student()
        {
            this.PartialGrades = new Dictionary<Subject, List<Grade>>();
            this.FinalGrades = new Dictionary<Subject, List<Grade>>();
        }
    }
}
