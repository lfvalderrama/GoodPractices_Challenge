using GoodPractices_Engine;
using GoodPractices_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RestApi.Controllers
{
    public class SubjectsController : ApiController
    {
        private readonly HttpConfiguration _config = GlobalConfiguration.Configuration;
        SubjectEngine _subjectsEngine;

        // PUT api/subjects/5
        public HttpResponseMessage Put(int id, [FromBody]Subject value)
        {
            var scope = _config.DependencyResolver.BeginScope();
            _subjectsEngine = scope.GetService(typeof(SubjectEngine)) as SubjectEngine;
            var result = _subjectsEngine.UpdateSubject(id, value);
            var code = HttpStatusCode.OK;
            if (result.Item1 == 404) code = HttpStatusCode.NotFound;
            return Request.CreateResponse(code, result.Item2);
        }
    }
}
