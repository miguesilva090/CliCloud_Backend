using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Common.Mailer
{
  public interface IMailService : ITransientService
  {
    Task SendAsync(MailRequest request);
  }
}
