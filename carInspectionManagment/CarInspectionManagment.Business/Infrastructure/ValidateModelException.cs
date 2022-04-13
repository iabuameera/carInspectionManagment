using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CarInspectionManagment.Business.Infrastructure
{


    [Serializable]
    public class ValidateModelException : System.Exception
    {
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
        public ValidateModelException()
        {
            // Empty constructor required to compile.
        }

        public ValidateModelException(string message)
            : base(message)
        {
        }

        public ValidateModelException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        public ValidateModelException(Dictionary<string, string> errors)
        {
            Errors = errors;
        }

        // The special constructor is used to deserialize values.
        protected ValidateModelException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Reset the property value using the GetValue method.
        }

        // Implement this method to serialize data. The method is called 
        // on serialization.
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            // Use the AddValue method to specify serialized values.
            info.AddValue("props", Errors, typeof(string));
        }

        public string GetErrorString(
            string key)
        {
            Errors.TryGetValue(key, out var errorString);
            return errorString;
        }

        public void Add(
            string key,
            string value)
        {
            Errors.Add(key, value);
        }

    }

    /// <summary>
    /// Throw Notfound result from the http layer. 404
    /// </summary>
    [Serializable]
    public class ItemNotFoundException : ValidateModelException
    {
        public ItemNotFoundException()
        {
        }

        public ItemNotFoundException(string message) : base(message)
        {
        }

        public ItemNotFoundException(Dictionary<string, string> errors) : base(errors)
        {
        }

        public ItemNotFoundException(
            string key,
            string error)
            : base(new Dictionary<string, string>())
        {
            Errors.Add(key, error);
        }

        public ItemNotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected ItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string Message
        {
            get
            {
                var builder = new StringBuilder();
                builder.AppendLine(base.Message);

                foreach (var error in Errors)
                {
                    builder.AppendLine($"{error.Key}: {string.Join(". ", error.Value)}.");
                }

                return builder.ToString();
            }
        }
    }

    /// <summary>
    /// Throw Badrequest from the http layer. 400
    /// </summary>
    [Serializable]
    public class InvalidModelException : ValidateModelException
    {
        public InvalidModelException()
        {
        }

        public InvalidModelException(string message) : base(message)
        {
        }

        public InvalidModelException(Dictionary<string, string> errors) : base(errors)
        {
        }

        public InvalidModelException(
            string key,
            string error)
            : base(new Dictionary<string, string>())
        {
            Errors.Add(key, error);
        }

        public InvalidModelException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidModelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class ConflictException : ValidateModelException
    {
        public ConflictException()
        {
        }

        public ConflictException(string message) : base(message)
        {
        }

        public ConflictException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected ConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        public ConflictException(
            string key,
            string error)
            : base(new Dictionary<string, string>())
        {
            Errors.Add(key, error);
        }

        public ConflictException(Dictionary<string, string> errors)
            : base(errors)
        {
            Errors = errors;
        }
    }



    [Serializable]
    public class UnauthorizedException : ValidateModelException
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        public UnauthorizedException(
            string key,
            string error)
            : base(new Dictionary<string, string>())
        {
            Errors.Add(key, error);
        }

        public UnauthorizedException(Dictionary<string, string> errors)
            : base(errors)
        {
            Errors = errors;
        }
    }
    /// <summary>
    /// Throw forbidden exception from http layer
    /// </summary>
    [Serializable]
    public class InvalidOperationException : ValidateModelException
    {
        public InvalidOperationException()
        {
        }

        public InvalidOperationException(string message) : base(message)
        {
        }

        public InvalidOperationException(Dictionary<string, string> errors) : base(errors)
        {
        }

        public InvalidOperationException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidOperationException(
            string key,
            string error)
            : base(new Dictionary<string, string>())
        {
            Errors.Add(key, error);
        }
    }

    [Serializable]
    public class InvalidEntityIdProvidedException : ValidateModelException
    {
        public InvalidEntityIdProvidedException()
        {
        }

        public InvalidEntityIdProvidedException(string message) : base(message)
        {
        }

        public InvalidEntityIdProvidedException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidEntityIdProvidedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        public InvalidEntityIdProvidedException(
            string key,
            string error)
            : base(new Dictionary<string, string>())
        {
            Errors.Add(key, error);
        }
    }

    //[Serializable]
    //public class UopeopleException<T> : ArgumentException,
    //  IUopeopleException
    //    where T : new()
    //{
    //    public UopeopleException()
    //    {
    //    }

    //    public UopeopleException(string message)
    //        : base(message)
    //    {
    //    }

    //    public UopeopleException(string message, System.Exception innerException)
    //        : base(message, innerException)
    //    {
    //    }

    //    protected UopeopleException(SerializationInfo info, StreamingContext context)
    //        : base(info, context)
    //    {
    //    }
    //    public T Errors { get; set; }

    //    public ValidationException ExceptionType { get; set; }

    //    public UopeopleException(ValidationException exception)
    //    {
    //        Errors = new T();
    //        ExceptionType = exception;
    //    }


    //    public object GetErrors()
    //    {
    //        return Errors;
    //    }
    //}

    //public interface IUopeopleException
    //{
    //    ValidationException ExceptionType { get; set; }
    //    object GetErrors();
    //}

    public enum ValidationException
    {
        ItemNotFound = 1,
        InvalidModel = 2,
        Conflict = 3,
        InvalidOperation = 4,
        ServerError = 5
    }



}
