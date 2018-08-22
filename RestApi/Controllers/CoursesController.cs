using GoodPractices_Engine;
using GoodPractices_Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GoodPractices_ResponseModel;

namespace RestApi.Controllers
{
    public class CoursesController : ApiController
    {
        private readonly HttpConfiguration _config = GlobalConfiguration.Configuration;
        CourseEngine _courseEngine;

        // GET api/values
        public List<ResponseCourse> Get()
        {
            var scope = _config.DependencyResolver.BeginScope();
            _courseEngine = scope.GetService(typeof(CourseEngine)) as CourseEngine;
            return _courseEngine.GetCourses();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
