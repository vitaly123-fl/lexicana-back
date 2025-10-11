using System.Net;
using System.Text.Json;

namespace lexicana.Endpoints
{
    public class EmptyValue{}

    public class FileNameResponse : EmptyValue
    {
        public string Url { get; set; }
        public string ContentType { get; set; }
        public string Name { get; set; }
    }
    
   public  class Response
    {
        public bool IsValid => Message is null || Message.Length == 0;
        public string[] Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public object AdditionalData { get; set; }

        public Response(HttpStatusCode statusCode, params string[] message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
    public class Response<T> :Response
    {
        public T Value { get; set; }

        public Response(HttpStatusCode statusCode, params string[] message):base(statusCode,message)
        {
        }
        public Response(T value, HttpStatusCode statusCode):base(statusCode)
        {
            Value = value;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(new {Message,StatusCode});
        }

        public Response<TRequest> Convert<TRequest>()
        {
            return new Response<TRequest>(StatusCode, Message);
        }
    }

    public static class SuccessResponses
    {
        public static Response<T> Created<T>(T value)
        {
            return new Response<T>(value, HttpStatusCode.Created);
        }

        public static Response<T> Ok<T>(T value)
        {
            return new Response<T>(value, HttpStatusCode.OK);
        }

        public static Response<EmptyValue> Ok()
        {
            return new Response<EmptyValue>(HttpStatusCode.OK);
        }

        public static Response<EmptyValue> NoContent()
        {
            return new Response<EmptyValue>(HttpStatusCode.NoContent);
        }
        public static Response<FileNameResponse> File(FileNameResponse response)
        {
            return new Response<FileNameResponse>(response,HttpStatusCode.OK);
        }
    }

    public static class FailureResponses
    {
        public static Response<T> NotFound<T>(string message)
        {
            return new Response<T>(HttpStatusCode.NotFound, message);
        }
        public static Response<EmptyValue> NotFound(string message)
        {
            return NotFound<EmptyValue>(message);
        }
        public static Response<T> Forbidden<T>(string message)
        {
            return new Response<T>(HttpStatusCode.Forbidden, message);
        }
        public static Response<EmptyValue> Forbidden(string message)
        {
            return Forbidden<EmptyValue>(message);
        }
        public static Response<T> BadRequest<T>(params string[] message)
        {
            return new Response<T>(HttpStatusCode.BadRequest, message);
        }
        public static T BadRequestResponse<T>(params string[] message) where T :Response
        {
            var result =(T)Activator.CreateInstance(typeof(T), new object[] { HttpStatusCode.BadRequest, message});
            return result;
        }

        public static Response<EmptyValue> BadRequest(params string[] message)
        {
            return BadRequest<EmptyValue>(message);
        }

        public static Response<EmptyValue> InternalError(params string[] message)
        {
            return InternalError<EmptyValue>(message);
        }
        public static Response<T> InternalError<T>(params string[] message)
        {
            return new Response<T>(HttpStatusCode.InternalServerError, message);
        }
        public static Response<T> UnAuthorized<T>()
        {
            return new Response<T>(HttpStatusCode.Unauthorized, "User are not authorized");
        }
        public static Response<EmptyValue> Conflict(string message)
        {
            return new Response<EmptyValue>(HttpStatusCode.Conflict, message);
        }

    }
}