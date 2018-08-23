using System.Net;

namespace RestApi.Helpers
{
    public interface ICodeHandler
    {
        HttpStatusCode GetStatusCode(int code);
    }
    public class CodeHandler : ICodeHandler
    {
        public HttpStatusCode GetStatusCode(int code)
        {
            switch (code)
            {
                case 200:
                    return HttpStatusCode.OK;
                case 201:
                    return HttpStatusCode.Created;
                case 400:
                    return HttpStatusCode.BadRequest;
                case 404:
                    return HttpStatusCode.NotFound;
                case 500:
                    return HttpStatusCode.InternalServerError;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }
    }
}