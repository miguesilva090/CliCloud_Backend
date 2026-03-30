using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Utility.ReportService;
using CliCloud.Application.Services.Utility.ReportService.DTOs;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Utility
{
    [Route("client/utility/reports")]
    [ApiController]
    public class ReportController(IReportService ReportService) : ControllerBase
    {
      private readonly IReportService _ReportService = ReportService;

      // Save Report

      [Authorize(Roles = "client")]
      [HttpPost]
      public async Task<IActionResult> SaveReportAsync(SaveReportRequest request)
      {
        try
        {
          Response<string> result = await _ReportService.SaveReportAsync(request);

          if(result.Status == ResponseStatus.Success)
          {
            return Ok(result);
          }

          if(result.Messages.TryGetValue("$", out var saveMessages)
          && saveMessages.Any(m => m.Contains("not found"))
          )
          {
            return NotFound(result);
          }

          return BadRequest(result);
        }
        catch(Exception ex)
        {
          Response<string> errorResponse = ResponseFactory.Fail<string>($"Failed to save report: {ex.Message}");

          return StatusCode(500, errorResponse);
        }
      }


      // Get All Reports
      [Authorize(Roles = "client")]
      [HttpGet]
      public async Task<IActionResult> GetAllReportsAsync()
      {
        try
        {
          Response<string[]> result = await _ReportService.GetAllReportsAsync();
          return Ok(result);
        }
        catch(Exception ex)
        {
          Response<string[]> errorResponse = ResponseFactory.Fail<string[]>($"Failed to get all reports: {ex.Message}");
          return StatusCode(500, errorResponse);
        }
      }

      // Get Original Reports
      [Authorize(Roles = "client")]
      [HttpPost("get-originals")]
      public async Task<IActionResult> GetOriginalReportsAsync()
      {
        try
        {
          Response<string[]> result = await _ReportService.GetOriginalReportsAsync();
          return Ok(result);
        }
        catch(Exception ex)
        {
          Response<string[]> errorResponse = ResponseFactory.Fail<string[]>($"Failed to get original reports: {ex.Message}");
          return StatusCode(500, errorResponse);
        }
      }

      // Revert Report
      [Authorize(Roles = "client")]
      [HttpPost("revert")]
      public async Task<IActionResult> RevertReportAsync([FromBody]RevertReportRequest request)
      {
        try
        {
          if(request == null || string.IsNullOrWhiteSpace(request.ReportName))
          {
            Response<string> errorResponse = ResponseFactory.Fail<string>("Report name is required");
            return BadRequest(errorResponse);
          }

          Response<string> result = await _ReportService.RevertReportAsync(request);

          if(result.Status == ResponseStatus.Failure
          && result.Messages.TryGetValue("$", out var messages)
          && messages.Any(m => m.Contains("not found in reports-originais folder"))
          )
          {
            return NotFound(result);
          }

          if(result.Status == ResponseStatus.Success)
          {
            return Ok(result);
          }

          return StatusCode(500, result);
        }
        catch(Exception ex)
        {
          Response<string> errorResponse = ResponseFactory.Fail<string>($"Failed to revert report: {ex.Message}");
          return StatusCode(500, errorResponse);
        }
      }

      //Delete Report 
      [Authorize(Roles = "client")]
      [HttpDelete("{reportName}")]
      public async Task<IActionResult> DeleteReportAsync(string reportName)
      {
        try
        {
          if(string.IsNullOrWhiteSpace(reportName))
          {
            Response<string> errorResponse = ResponseFactory.Fail<string>("Report name is required");
            return BadRequest(errorResponse);
          }

          Response<string> result = await _ReportService.DeleteReportAsync(reportName);

          if( result.Status == ResponseStatus.Failure
          && result.Messages.TryGetValue("$", out var messages)
          && messages.Any(m => m.Contains("not found")))
          {
            return NotFound(result);
          }

          if(result.Status == ResponseStatus.Success)
          {
            return Ok(result);
          }

          return StatusCode(500, result);
        }
        catch(Exception ex)
        {
          Response<string> errorResponse = ResponseFactory.Fail<string>($"Failed to delete report: {ex.Message}");

          return StatusCode(500, errorResponse);
        }
      }

      // Get Report 
      [Authorize(Roles = "client")]
      [HttpGet("{reportName}")]
      public async Task<IActionResult> GetReportAsync(string reportName)
      {
        try
        {
          Response<string> result = await _ReportService.GetReportAsync(reportName);

          if(result.Status == ResponseStatus.Success)
          {
            return Ok(result);
          }

          if( result.Messages.TryGetValue("$", out var getMessages)
          && getMessages.Any(m => m.Contains("not found"))
          )
          {
            return NotFound(result);
          }
          return BadRequest(result);
        }
        catch(Exception ex)
        {
          Response<string> errorResponse = ResponseFactory.Fail<string>($"Failed to retrieve report: {ex.Message}");
          return StatusCode(500, errorResponse);
        }
      }
    }
}
