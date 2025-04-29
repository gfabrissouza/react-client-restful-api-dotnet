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

        [HttpPost("upload-file")]
        [ProducesResponseType(200, Type = typeof(FileDetailVO))]
        [ProducesResponseType(400, Type = typeof(FileDetailVO))]
        [ProducesResponseType(401, Type = typeof(FileDetailVO))]
        [Produces("application/json")]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest request)
        {
            FileDetailVO detail = await _fileBusiness.SaveFileToDisk(request.File);
            return new OkObjectResult(detail);
        }
    }
}
