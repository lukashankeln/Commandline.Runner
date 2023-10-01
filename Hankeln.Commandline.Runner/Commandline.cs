using Hankeln.Commandline.Runner.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Hankeln.Commandline.Runner.CommandlineResult;

namespace Hankeln.Commandline.Runner
{
  /// <summary>
  /// An Object defining and executing the Commandline
  /// </summary>
  public sealed class Commandline
  {
    /// <summary> The File to be executed </summary>
    public string FileName { get; }

    /// <summary> The Arguments to be given to the process on start </summary>
    public List<string> Arguments { get; }

    /// <summary> Options defining the behavior for the commandline </summary>
    public CommandlineOptions Options { get; }

    /// <summary> The logger used to log the process </summary>
    public ICommandlineLogging Logger { get; }

    internal Commandline(string fileName, List<string> arguments, CommandlineOptions options, ICommandlineLogging logger)
    {
      FileName = fileName;
      Arguments = arguments;
      Options = options;
      Logger = logger;
    }

    private async Task<CommandlineResult> RunInternAsync(CancellationToken cancellationToken)
    {
      var args = Arguments != null ? string.Join(" ", Arguments) : "";
      var info = new ProcessStartInfo
      {
        FileName = FileName,
        Arguments = args,
        RedirectStandardOutput = Options.WriteLog,
        RedirectStandardError = Options.WriteLog,
        UseShellExecute = false // needs to be false for std-redirect
      };

      if (Options.WorkingDirectory is not null)
        info.WorkingDirectory = Options.WorkingDirectory.FullName;

      if (Options.WriteLog)
      {
        info.StandardOutputEncoding = Options.Encoding ?? Encoding.UTF8;
        info.StandardErrorEncoding = Options.Encoding ?? Encoding.UTF8;
      }

      Logger.Message($"[{FileName}] -> {args}");

      var sw = Stopwatch.StartNew();
      var process = Process.Start(info);

      if (process is null)
        return new CommandlineResult()
        {
          State = CommandlineState.Faulted,
          ExitCode = Faulted
        };

      if (!Options.WaitForExit)
      {
        var res = new CommandlineResult() { State = CommandlineState.Running };
        process.Exited += (object? sender, System.EventArgs e) =>
        {
          res.State = CommandlineState.Finished;
        };
        return res;
      }

      if (Options.WriteLog)
      {
        process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
        {
          if (!string.IsNullOrEmpty(e.Data))
          {
            Logger.StdOut(e.Data.Trim());
          }
        };

        process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
        {
          if (!string.IsNullOrEmpty(e.Data))
          {
            Logger.ErrOut(e.Data.Trim());
          }
        };

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
      }

      while (!process.HasExited)
      {
        if (cancellationToken.IsCancellationRequested || (Options.Timeout.HasValue && sw.Elapsed > Options.Timeout))
        {
          process.Kill();
          Logger.Message($"[{FileName}] -> killed");
          return new CommandlineResult()
          {
            State = CommandlineState.Timeout,
            ExitCode = process.ExitCode
          };
        }

        await Task.Delay(500);
      }

      Logger.Message($"[{FileName}] -> Exitcode: {process.ExitCode}");
      return new CommandlineResult()
      {
        State = CommandlineState.Finished,
        ExitCode = process.ExitCode
      };

    }

    /// <inheritdoc cref="RunAsync(CancellationToken)"/>
    public Task<CommandlineResult> RunAsync() => RunAsync(CancellationToken.None);

    /// <summary>
    /// Executes the given Commandline
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the running process</param>
    /// <returns>An result object for the running Commandline</returns>
    public async Task<CommandlineResult> RunAsync(CancellationToken cancellationToken)
    {
      try
      {
        return await RunInternAsync(cancellationToken);
      }
      catch (System.Exception e)
      {
        if (Options.WriteLog)
          Logger.Message($"[ERR {FileName}] -> {e.Message}");
        return new CommandlineResult()
        {
          ExitCode = Faulted,
          State = CommandlineState.Faulted
        };
      }
    }
  }
}
