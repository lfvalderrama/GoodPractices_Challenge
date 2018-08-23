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
using System.Web.Services.Description;

namespace RestApi.Controllers
{
    public class CourseInput
    {
        public string Name { get; set; }
        public string HeadmanDocument { get; set; }
        public string TeacherDocument { get; set; }
    }
    public class CoursesController : ApiController
    {
        private readonly HttpConfiguration _config = GlobalConfiguration.Configuration;
        CourseEngine _courseEngine;

        // GET api/courses
        public List<ResponseCourse> Get()
        {
            var scope = _config.DependencyResolver.BeginScope();
            _courseEngine = scope.GetService(typeof(CourseEngine)) as CourseEngine;
            return _courseEngine.GetCourses();
        }

        // GET api/courses/5
        public HttpResponseMessage Get(long id)
        {
            var scope = _config.DependencyResolver.BeginScope();
            _courseEngine = scope.GetService(typeof(CourseEngine)) as CourseEngine;
            var code = HttpStatusCode.OK;
            var result = _courseEngine.GetCourseById(id);
            if(result.Item1 == 404) code = HttpStatusCode.NotFound;
            return Request.CreateResponse(code, result.Item2);
        }

        // POST api/courses
        public HttpResponseMessage Post([FromBody]CourseInput value)
        {
            var scope = _config.DependencyResolver.BeginScope();
            _courseEngine = scope.GetService(typeof(CourseEngine)) as CourseEngine;
            var result = _courseEngine.CreateCourse(value.Name, value.HeadmanDocument, value.TeacherDocument);
            var code = HttpStatusCode.Created;
            if (result.Item1 != 201) code = HttpStatusCode.BadRequest;
            return Request.CreateResponse(code, result.Item2);
        }

            // PUT api/courses/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/courses/5
        public void Delete(int id)
        {
        }
    }
}
