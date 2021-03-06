using System.Net;

namespace TOP.IdentityService.Domain.Models
{
    public class InnerError
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Status { get; set; }

        public static InnerError Default() => new InnerError
        {
            Title = "unexpected error",
            Detail = "an unexpected error occured",
            Status = ((int)HttpStatusCode.InternalServerError).ToString()
        };
    }
}
