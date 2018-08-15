using System;
using System.Collections.Generic;

namespace GoodPractices_Controller
{
    public interface IValidation
    {
        String CheckExistence (Dictionary<String, String> input);
    }
}
