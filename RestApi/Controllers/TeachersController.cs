using GoodPractices_Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RestApi.Controllers
{
    public class TeachersController : ApiController
    {
        private readonly HttpConfiguration _config = GlobalConfiguration.Configuration;
        TeacherEngine _teacherEngine;

        // DELETE api/teachers/5
        public HttpResponseMessage Delete(int id)
        {
            var scope = _config.DependencyResolver.BeginScope();
            _teacherEngine = scope.GetService(typeof(TeacherEngine)) as TeacherEngine;
            var result = _teacherEngine.DeleteTeacher(id.ToString());
            var code = HttpStatusCode.OK;
            if (result.Item1 == 400) code = HttpStatusCode.BadRequest;
            if (result.Item1 == 404) code = HttpStatusCode.NotFound;
            return Request.CreateResponse(code, result.Item2);
        }
    }
}
