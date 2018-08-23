using System;
using System.Collections.Generic;

namespace GoodPractices_Engine
{
    public interface IValidation
    {
        String CheckExistence (Dictionary<String, String> input);
    }
}
