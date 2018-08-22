using System;
using System.Collections.Generic;

namespace GoodPractices_Model
{
    public class Subject
    {
        public Subject(string name, string content)
        {
            this.Name = name;
            this.Content = content;
        }
        
        public List<Teacher> Teachers { get; set; }
        public List<Course> Courses { get; set; }
        public int Id { get; set; }
        public String Name { get; set; }
        public String Content { get; set; }

        public Subject()
        {
        }
    }
}
