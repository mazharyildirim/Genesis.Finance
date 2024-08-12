using System.Net;

namespace Genesis.CoreApi.Shared.Exception
{

    public class OperationException : System.Exception, IGenesisApiException
    {
        public OperationException(string message) : base(message)
        {
            base.Data.Add("Message", "Operation Exception");
            base.Data.Add("HttpStatusCode", HttpStatusCode.InternalServerError);
        }
    }

    public class DataMissingException : System.Exception, IGenesisApiException
    {
        public DataMissingException(string message) : base(message)
        {
            base.Data.Add("Message", "Data Missing Exception");
            base.Data.Add("HttpStatusCode", HttpStatusCode.BadRequest);
        }
    }

    public class AuthorizationException : System.Exception, IGenesisApiException
    {
        public AuthorizationException(string message) : base(message)
        {
            base.Data.Add("Message", "Not Authorized");
            base.Data.Add("HttpStatusCode", HttpStatusCode.Unauthorized);
        }
    }

    public class ValidationException : System.Exception, IGenesisApiException
    {
        public ValidationException(string message) : base(message)
        {
            base.Data.Add("Message", "Validation Exception");
            base.Data.Add("HttpStatusCode", HttpStatusCode.BadRequest);
        }
    }

    public class ServiceRegistrationException : System.Exception, IGenesisApiException
    {
        public ServiceRegistrationException(string message) : base(message)
        {
            base.Data.Add("Message", "Validation Exception");
            base.Data.Add("HttpStatusCode", HttpStatusCode.BadRequest);
        }
    }
}

