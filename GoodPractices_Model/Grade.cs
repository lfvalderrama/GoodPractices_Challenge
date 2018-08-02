using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodPractices_Challenge
{
    class Grade
    {
        public int Id { set; get; }
        public String Period { set; get; }
        public float Score { set; get; }

        public Grade(string period, float score)
        {
            this.Period = period;
            this.Score = score;
        }
    }
}
