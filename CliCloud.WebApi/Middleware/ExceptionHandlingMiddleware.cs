using System.Net;
using System.Security.Cryptography;
using CliCloud.Application.Common.Errors;
using CliCloud.Application.Common.Logging;
using CliCloud.Application.Common.Wrapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CliCloud.WebApi.Middleware
{
  public class ExceptionHandlingMiddleware(RequestDelegate next, AppLogger appLogger)
  {
    private readonly RequestDelegate _next = next;
    private readonly AppLogger _appLogger = appLogger; // Inject AppLogger

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        // Check for model binding errors before proceeding
        if (
          !httpContext.Request.HasJsonContentType()
          && httpContext.Response.StatusCode == StatusCodes.Status400BadRequest
        )
        {
          ModelStateDictionary? modelState = httpContext.Features.Get<ModelStateDictionary>();
          if (modelState != null && !modelState.IsValid)
          {
            Dictionary<string, List<string>> errorMessages = [];
            foreach ((string key, ModelStateEntry value) in modelState)
            {
              if (value.Errors.Count > 0)
              {
                errorMessages[key] = [.. value.Errors.Select(e => e.ErrorMessage)];
              }
            }

            Response response = new() { Status = ResponseStatus.Failure, Messages = errorMessages };
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(response);
            return;
          }
        }

        await _next(httpContext);
      }
      catch (BadRequestException badRequestEx)
      {
        // Log validation error details
        _appLogger.LogWarningWithException(
          "[MIDDLEWARE][VALIDATION] Falha na validação.",
          badRequestEx
        );

        // Return custom validation error response
        Response response = new()
        {
          Status = ResponseStatus.Failure,
          Messages = badRequestEx.Errors,
          
        };

        // Set status code for BadRequest and return the validation error messages as JSON
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
        await httpContext.Response.WriteAsJsonAsync(response);
        return; // Prevents other middlewares from handling the response
      }
      catch (CryptographicException cryptographicEx)
      {
        // Log the cryptographic exception
        _appLogger.LogErrorWithException(
          "[MIDDLEWARE][CRYPTO] Erro criptográfico: chave inválida ou problema de criptografia.",
          cryptographicEx
        );

        // Return a custom error response for CryptographicException
        Response response = Response.Fail(
          "Ocorreu um erro interno. Por favor, tente novamente mais tarde."
        );

        // Set status code for BadRequest and return the error message as JSON
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
        await httpContext.Response.WriteAsJsonAsync(response);
      }
      catch (InvalidOperationException invalidOpEx)
      {
        _appLogger.LogWarningWithException(
          "[MIDDLEWARE][INVALID_OPERATION] Operação inválida.",
          invalidOpEx
        );

        Response response = Response.Fail(invalidOpEx.Message);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await httpContext.Response.WriteAsJsonAsync(response);
      }
      catch (FileNotFoundException fileNotFoundEx)
      {
        _appLogger.LogWarningWithException(
          "[MIDDLEWARE][FILE_NOT_FOUND] Arquivo não encontrado.",
          fileNotFoundEx
        );

        // Evita expor caminhos internos do servidor para o cliente.
        Response response = Response.Fail("O arquivo solicitado não foi encontrado.");

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
        await httpContext.Response.WriteAsJsonAsync(response);
      }
      catch (Exception ex)
      {
        // Log unexpected error
        _appLogger.LogErrorWithException(
          "[MIDDLEWARE][UNHANDLED] Ocorreu um erro inesperado.",
          ex
        );

        // Return a generic error response for other exceptions
        Response response = Response.Fail(
          "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."
        );

        // Set status code for InternalServerError and return a generic error message
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
        await httpContext.Response.WriteAsJsonAsync(response);
      }
    }
  }
}
