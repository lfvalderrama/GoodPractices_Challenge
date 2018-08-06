using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodPractices_Model
{
    public class Grade
    {
        public int Id { set; get; }
        public String Period { set; get; }
        public float Score { set; get; }
        public Subject Subject { get; set;}
        public GradeType Type { get; set; }

        public Grade()
        {
        }

        public Grade(string period, float score, Subject subject, GradeType type)
        {
            this.Period = period;
            this.Score = score;
            this.Subject = subject;
            this.Type = type;
        }
    }
}
