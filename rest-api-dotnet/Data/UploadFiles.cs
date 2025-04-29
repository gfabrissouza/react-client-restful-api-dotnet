using System.ComponentModel.DataAnnotations;

namespace RestApiDotNet.Data
{
    public class UploadFileRequest
    {
        [Required]
        public IFormFile File { get; set; }
    }

    public class UploadFilesRequest
    {
        [Required]
        public List<IFormFile> Files { get; set; }
    }
}
