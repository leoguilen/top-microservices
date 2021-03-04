using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.Serialization;
using TOP.IdentityService.Domain.Models;

namespace TOP.IdentityService.Domain.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class CustomException : Exception
    {
        public CustomException(string message) : base(message)
        {
        }

        public CustomException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public virtual string Title => "unexpected error";
        public virtual string Detail => "an unexpected error ocurred";
        public virtual HttpStatusCode StatusCode  => HttpStatusCode.InternalServerError;

        public virtual Error Err => new Error
        {
            Errors = ImmutableList.Create(new InnerError
            {
                Title = Title,
                Detail = Detail,
                Status = ((int)StatusCode).ToString()
            })
        };
    }
}
