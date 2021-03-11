using Microsoft.AspNetCore.Http;
using System.IO;

namespace TOP.ProfileService.Domain.Extensions
{
    public static class ByteExtensions
    {
        public static byte[] FormFileToByteArray(this IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
