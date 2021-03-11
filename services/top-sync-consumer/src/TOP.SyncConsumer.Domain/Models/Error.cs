using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;

namespace TOP.SyncConsumer.Domain.Models
{
    public class Error
    {
        public IEnumerable<InnerError> Errors { get; set; }

        [JsonIgnore]
        public int StatusCode
        {
            get
            {
                var status = Errors.FirstOrDefault()?.Status;

                if (string.IsNullOrWhiteSpace(status))
                {
                    return (int)HttpStatusCode.InternalServerError;
                }

                return int.TryParse(status, out var result)
                    ? result
                    : (int)HttpStatusCode.InternalServerError;
            }
        }

        public static Error Default() => new Error
        {
            Errors = ImmutableList.Create(InnerError.Default())
        };
    }
}
