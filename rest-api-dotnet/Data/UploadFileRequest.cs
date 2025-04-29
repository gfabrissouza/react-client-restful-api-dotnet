using System.ComponentModel.DataAnnotations;

namespace RestApiDotNet.Data
{
    public class UploadFileRequest
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
