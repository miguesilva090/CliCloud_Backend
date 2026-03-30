using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Images;
using Microsoft.AspNetCore.Hosting;

namespace CliCloud.WebApi.Controllers.Utility
{
    [Route("client/utility/[controller]")]
    [ApiController]
    public class ImageUploadController(IWebHostEnvironment environment) : ControllerBase
    {
        private readonly IWebHostEnvironment _environment = environment;

        //Upload Image 
        [Authorize(Roles = "client")]
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImageAsync(UploadImageRequest request)
        {
          try
          {
            if(request.File == null || request.File.Length == 0)
            {
              Response<UploadImageResponse> errorResponse = ResponseFactory.Fail<UploadImageResponse>("O ficheiro é obrigatório");
              return BadRequest(errorResponse);
            }

            string uploadsFolder = Path.Combine(_environment.WebRootPath, "assets", "imagens");
            
            if(!string.IsNullOrWhiteSpace(request.Subfolder))
            {
              string sanitazedSubfolder = request.Subfolder.Trim();
              uploadsFolder = Path.Combine(uploadsFolder, sanitazedSubfolder);
            }

            if(!Directory.Exists(uploadsFolder))
            {
              _ = Directory.CreateDirectory(uploadsFolder);
            }

            string fileExtension = Path.GetExtension(request.File.FileName);
            string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using(FileStream stream = new (filePath, FileMode.Create))
            {
              await request.File.CopyToAsync(stream);
            }

            string imageUrl = string.IsNullOrWhiteSpace(request.Subfolder)
              ? $"/assets/imagens/{uniqueFileName}"
              : $"/assets/imagens/{request.Subfolder.Trim()}/{uniqueFileName}";

            UploadImageResponse response = new() { Url = imageUrl };
            Response<UploadImageResponse> result = ResponseFactory.Success(response);
            return Ok(result);
          }
          catch(Exception ex)
          {
            Response<UploadImageResponse> errorResponse = ResponseFactory.Fail<UploadImageResponse>(ex.Message);
            return BadRequest(errorResponse);
          }
        }
    }
}
