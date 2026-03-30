using CliCloud.Application.Common.Marker;
using Microsoft.AspNetCore.Http;

namespace CliCloud.Application.Common.Images
{
  public interface IImageService : ITransientService
  {
    Task<string> AddImage(IFormFile file, int height, int width);
    Task<string> DeleteImage(string url);
  }
}
