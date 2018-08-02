using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodPractices_Model
{
    public class ForeignLanguage : Subject
    {

        public Language Language { get; set; }

        public ForeignLanguage()
        {
        }

        public ForeignLanguage(Language language, string name, string content):base(name,content)
        {
            this.Language = language;
            this.Name = name;
            this.Content = Content;
        }


    }
}
