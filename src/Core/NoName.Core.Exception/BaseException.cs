using System.Net;

namespace NoName.Core.Exception
{
    public abstract class BaseException : System.Exception
    {
        private HttpStatusCode _httpStatusCode;

        public BaseException(HttpStatusCode httpStatusCode)
        {
            _httpStatusCode = httpStatusCode;
        }

        public HttpStatusCode GetHttpResponseCode()
        {
            return _httpStatusCode;
        }

        public abstract object GenerateExceptionModel();
    }
}