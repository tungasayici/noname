using System.Net;

namespace NoName.Core.Exception
{
    public class BusinessException : BaseException
    {
        private string _errorCode;
        private string _errorDescription;

        public BusinessException(string errorCode, string errorDescription) : base(HttpStatusCode.InternalServerError)
        {
            _errorCode = errorCode;
            _errorDescription = errorDescription;
        }

        public BusinessException(HttpStatusCode httpStatusCode, string errorCode, string errorDescription) : base(httpStatusCode)
        {
            _errorCode = errorCode;
            _errorDescription = errorDescription;
        }

        public override object GenerateExceptionModel()
        {
            return new { ErrorCode = _errorCode, ErrorDescription = _errorDescription };
        }
    }
}