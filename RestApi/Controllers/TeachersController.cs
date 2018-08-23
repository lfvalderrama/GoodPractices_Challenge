using GoodPractices_Engine;
using GoodPractices_Model;
using RestApi.Helpers;
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
        private TeacherEngine _teacherEngine;
        private ICodeHandler _codeHandler;

        public TeachersController(ICodeHandler codeHandler, TeacherEngine teacherEngine)
        {
            _teacherEngine = teacherEngine;
            _codeHandler = codeHandler;
        }

        // DELETE api/teachers/5
        public HttpResponseMessage Delete(string id)
        {
            var result = _teacherEngine.DeleteTeacher(id);
            var code = _codeHandler.GetStatusCode(result.Item1);
            return Request.CreateResponse(code, result.Item2);
        }

        // PUT api/teachers/5
        public HttpResponseMessage Put(int id, [FromBody]Teacher value)
        {
            var result = _teacherEngine.UpdateTeacher(id, value);
            var code = _codeHandler.GetStatusCode(result.Item1);
            return Request.CreateResponse(code, result.Item2);
        }
    }
}
