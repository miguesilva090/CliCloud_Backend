namespace CliCloud.WebApi.Logging
{
  /// <summary>
  /// Provides logging functionality for application startup and database initialization.
  /// All logs are written to console (stdout) which is captured by IIS for stdout logging.
  /// </summary>
  public static class StartupLogger
  {
    /// <summary>
    /// Logs startup information messages
    /// </summary>
    public static void LogInfo(string message)
    {
      Console.WriteLine($"[STARTUP INFO] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC - {message}");
      Console.Out.Flush();
    }

    /// <summary>
    /// Logs startup error messages with optional exception details
    /// </summary>
    public static void LogError(string message, Exception? ex = null)
    {
      Console.WriteLine($"[STARTUP ERROR] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC - {message}");
      if (ex != null)
      {
        Console.WriteLine($"[STARTUP ERROR] Exception Type: {ex.GetType().Name}");
        Console.WriteLine($"[STARTUP ERROR] Exception Message: {ex.Message}");
        Console.WriteLine($"[STARTUP ERROR] Stack Trace: {ex.StackTrace}");
        if (ex.InnerException != null)
        {
          Console.WriteLine(
            $"[STARTUP ERROR] Inner Exception: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}"
          );
        }
      }
      Console.Out.Flush();
    }

    /// <summary>
    /// Logs database-related information messages
    /// </summary>
    public static void LogDatabaseInfo(string message)
    {
      Console.WriteLine($"[DATABASE INFO] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC - {message}");
      Console.Out.Flush();
    }

    /// <summary>
    /// Logs database-related error messages with optional exception details
    /// </summary>
    public static void LogDatabaseError(string message, Exception? ex = null)
    {
      Console.WriteLine($"[DATABASE ERROR] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC - {message}");
      if (ex != null)
      {
        Console.WriteLine($"[DATABASE ERROR] Exception Type: {ex.GetType().Name}");
        Console.WriteLine($"[DATABASE ERROR] Exception Message: {ex.Message}");
        Console.WriteLine($"[DATABASE ERROR] Stack Trace: {ex.StackTrace}");
        if (ex.InnerException != null)
        {
          Console.WriteLine(
            $"[DATABASE ERROR] Inner Exception: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}"
          );
        }
      }
      Console.Out.Flush();
    }

    /// <summary>
    /// Writes a startup error to a file in the logs directory for additional debugging
    /// </summary>
    public static void WriteErrorToFile(Exception ex, string? additionalContext = null)
    {
      try
      {
        string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        if (!Directory.Exists(logDir))
        {
          Directory.CreateDirectory(logDir);
        }
        string logFile = Path.Combine(
          logDir,
          $"startup-error-{DateTime.UtcNow:yyyyMMdd-HHmmss}.txt"
        );
        string logContent =
          $"[STARTUP ERROR] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC{Environment.NewLine}";

        if (!string.IsNullOrEmpty(additionalContext))
        {
          logContent += $"Context: {additionalContext}{Environment.NewLine}";
        }

        logContent +=
          $"Exception Type: {ex.GetType().Name}{Environment.NewLine}"
          + $"Exception Message: {ex.Message}{Environment.NewLine}"
          + $"Stack Trace: {ex.StackTrace}{Environment.NewLine}";

        if (ex.InnerException != null)
        {
          logContent +=
            $"Inner Exception: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}{Environment.NewLine}";
        }

        File.WriteAllText(logFile, logContent);
      }
      catch
      {
        // Ignore file logging errors - console logging is more important
      }
    }
  }
}
