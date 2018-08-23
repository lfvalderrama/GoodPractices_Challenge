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
using RestApi.Helpers;

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
        ICodeHandler _codeHandler;
        CourseEngine _courseEngine;

        public CoursesController(ICodeHandler codeHandler, CourseEngine courseEngine)
        {
            _courseEngine = courseEngine;
            _codeHandler = codeHandler;
        }

        // GET api/courses
        public HttpResponseMessage Get()
        {            
            var result = _courseEngine.GetCourses();
            var code = _codeHandler.GetStatusCode(result.Item1);
            return  (code != HttpStatusCode.InternalServerError) ?  Request.CreateResponse(code, result.Item2) : Request.CreateResponse(code, result.Item3);
        }

        // GET api/courses/5
        public HttpResponseMessage Get(long id)
        {
            var result = _courseEngine.GetCourseById(id);
            var code = _codeHandler.GetStatusCode(result.Item1);
            return (code != HttpStatusCode.InternalServerError) ? Request.CreateResponse(code, result.Item2) : Request.CreateResponse(code, result.Item3);
        }

        // POST api/courses
        public HttpResponseMessage Post([FromBody]CourseInput value)
        {
            var result = _courseEngine.CreateCourse(value.Name, value.HeadmanDocument, value.TeacherDocument);
            var code = _codeHandler.GetStatusCode(result.Item1);
            return Request.CreateResponse(code, result.Item2);
        }
    }
}
