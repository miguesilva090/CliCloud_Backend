using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.ReportService.DTOs;

namespace CliCloud.Application.Services.Utility.ReportService
{
    public interface IReportService : ITransientService
    {
        Task<Response<string>> SaveReportAsync(SaveReportRequest request);
        Task<Response<string>> GetReportAsync(string reportName);
        Task<Response<string>> RevertReportAsync(RevertReportRequest request);
        Task<Response<string[]>> GetAllReportsAsync();
        Task<Response<string[]>> GetOriginalReportsAsync();
        Task<Response<string>> DeleteReportAsync(string reportName);
    }
}
