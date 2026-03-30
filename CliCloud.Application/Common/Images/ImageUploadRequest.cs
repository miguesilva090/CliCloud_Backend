using CliCloud.Application.Common.Marker;
using Microsoft.AspNetCore.Http;

namespace CliCloud.Application.Common.Images
{
  public class ImageUploadRequest : IDto
  {
    public IFormFile ImageFile { get; set; } = null!;
    public bool DeleteCurrentImage { get; set; }
  }
}
