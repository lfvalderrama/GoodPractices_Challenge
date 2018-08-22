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
        public ResponseCourse Get(long id)
        {
            var scope = _config.DependencyResolver.BeginScope();
            _courseEngine = scope.GetService(typeof(CourseEngine)) as CourseEngine;
            return _courseEngine.GetCourseById(id);
        }

        // POST api/courses
        public ResponseMessage Post([FromBody]CourseInput value)
        {
            var scope = _config.DependencyResolver.BeginScope();
            _courseEngine = scope.GetService(typeof(CourseEngine)) as CourseEngine;
            var result = _courseEngine.CreateCourse(value.Name, value.HeadmanDocument, value.TeacherDocument);
            if (result.Code != 200)
            {
                return BadRequestResult(result);
            }
            else
            {
                return Ok(result);
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
