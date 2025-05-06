using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiDotNet.Business;
using RestApiDotNet.Data;
using RestApiDotNet.Data.VO;

namespace RestApiDotNet.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class FileController : ControllerBase
    {
        private readonly IFileBusiness _fileBusiness;

        public FileController(IFileBusiness fileBusiness)
        {
            _fileBusiness =  fileBusiness;
        }

        [HttpPost("download-file/{filename}")]
        [ProducesResponseType(200, Type = typeof(byte[]))]
        [ProducesResponseType(204, Type = typeof(byte[]))]
        [ProducesResponseType(400, Type = typeof(byte[]))]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> GetFileAsync(string fileName)
        {
            byte[] buffer = _fileBusiness.GetFile(fileName);
            if (buffer == null) {
                HttpContext.Response.ContentType = 
                    $"application/{Path.GetExtension(fileName).Replace(".", string.Empty)}";
                HttpContext.Response.Headers.Add("content-length", buffer.Length.ToString());
                await HttpContext.Response.Body.WriteAsync(buffer, 0, buffer.Length);
            }
            return new ContentResult();
        }

        [HttpPost("upload-file")]
        [ProducesResponseType(200, Type = typeof(FileDetailVO))]
        [ProducesResponseType(400, Type = typeof(FileDetailVO))]
        [ProducesResponseType(401, Type = typeof(FileDetailVO))]
        [Produces("application/json")]
        public async Task<IActionResult> UploadFileAsync([FromForm] UploadFileRequest file)
        {
            FileDetailVO detail = await _fileBusiness.SaveFileToDisk(file.File);
            return new OkObjectResult(detail);
        }

        [HttpPost("upload-files")]
        [ProducesResponseType(200, Type = typeof(FileDetailVO))]
        [ProducesResponseType(400, Type = typeof(FileDetailVO))]
        [ProducesResponseType(401, Type = typeof(FileDetailVO))]
        [Produces("application/json")]
        public async Task<List<FileDetailVO>> UploadFilesAsync([FromForm] UploadFilesRequest files)
        {
            List<FileDetailVO> list = [];
            foreach (var file in files.Files)
            {
                list.Add(await _fileBusiness.SaveFileToDisk(file));
            }
            return list;
        }
    }
}
