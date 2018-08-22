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
        public List<Info> Students { get; set; }
        public List<Info> Subjects { get; set; }
        public Info Headman { get; set; }
        public String Name { get; set; }

        public struct Info
        {
            public string Name { get; set; }
            public long Id { get; set; }
        }
    }

    
}
