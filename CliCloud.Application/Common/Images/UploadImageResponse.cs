namespace CliCloud.Application.Common.Images
{
  public class UploadImageResponse
  {
    public required string Url { get; set; }
    public string ImageUrl => Url;
    public string FileUrl => Url;
  }
}

