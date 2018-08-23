using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodPractices_ResponseModel
{
    public class ResponseCourse
    {
        public int Id { get; set; }
        public List<InfoStudent> Students { get; set; }
        public List<InfoSubject> Subjects { get; set; }
        public InfoStudent Headman { get; set; }
        public String Name { get; set; }

        public struct InfoStudent
        {
            public string Name { get; set; }
            public long Id { get; set; }
            public string Document { get; set; }
        }

        public struct InfoSubject
        {
            public string Name { get; set; }
            public long Id { get; set; }
        }
    }

    
}
