using System.Text.RegularExpressions;
using FluentValidation;
using CliCloud.Application.Common.Marker;
using Microsoft.AspNetCore.Http;

namespace CliCloud.Application.Common.Images
{
  public class UploadImageRequest : IDto
  {
    public required IFormFile File { get; set; }
    public string? Subfolder { get; set; }
  }

  public partial class UploadImageRequestValidator : AbstractValidator<UploadImageRequest>
  {
    private static readonly long MaxFileSize = 5 * 1024 * 1024; // 5MB
    private static readonly string[] AllowedExtensions =
    [
      ".jpg",
      ".jpeg",
      ".png",
      ".gif",
      ".webp",
      ".bmp",
    ];
    private static readonly string[] AllowedMimeTypes =
    [
      "image/jpeg",
      "image/jpg",
      "image/png",
      "image/gif",
      "image/webp",
      "image/bmp",
    ];

    [GeneratedRegex(@"^[a-zA-Z0-9-_]+$", RegexOptions.Compiled)]
    private static partial Regex SafeFolderNameRegex();

    public UploadImageRequestValidator()
    {
      _ = RuleFor(x => x.File)
        .NotNull()
        .WithMessage("O ficheiro é obrigatório")
        .Must(file => file.Length > 0)
        .WithMessage("O ficheiro não pode estar vazio")
        .Must(file => file.Length <= MaxFileSize)
        .WithMessage($"O tamanho do ficheiro não pode exceder {MaxFileSize / 1024 / 1024}MB")
        .Must(file =>
          AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant())
        )
        .WithMessage(
          $"Apenas são permitidos os seguintes formatos: {string.Join(", ", AllowedExtensions)}"
        )
        .Must(file => AllowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
        .WithMessage($"O tipo de ficheiro não é válido. Apenas imagens são permitidas.");

      _ = RuleFor(x => x.Subfolder)
        .Must(subfolder =>
          string.IsNullOrEmpty(subfolder) || SafeFolderNameRegex().IsMatch(subfolder)
        )
        .WithMessage("O nome da subpasta deve conter apenas letras, números, hífens e underscores")
        .Must(subfolder => string.IsNullOrEmpty(subfolder) || subfolder.Length <= 50)
        .WithMessage("O nome da subpasta não pode exceder 50 caracteres");
    }
  }
}
