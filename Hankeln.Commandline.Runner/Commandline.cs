using Hankeln.Commandline.Runner.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using static Hankeln.Commandline.Runner.CommandlineResult;

namespace Hankeln.Commandline.Runner
{
  public sealed class Commandline
  {
    public string FileName { get; }
    public List<string> Arguments { get; }
    public CommandlineOptions Options { get; }
    public ICommandlineLogging Logger { get; private set; }

    internal Commandline(string fileName, List<string> arguments, CommandlineOptions options, ICommandlineLogging logger)
    {
      FileName = fileName;
      Arguments = arguments;
      Options = options;
      Logger = logger;
    }

    private async Task<CommandlineResult> RunInternAsync()
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
        await Task.Delay(500);

        if (Options.Timeout.HasValue && sw.Elapsed > Options.Timeout)
        {
          process.Kill();
          Logger.Message($"[{FileName}] -> killed by Timeout");
          return new CommandlineResult()
          {
            State = CommandlineState.Timeout,
            ExitCode = process.ExitCode
          };
        }
      }

      Logger.Message($"[{FileName}] -> Exitcode: {process.ExitCode}");
      return new CommandlineResult()
      {
        State = CommandlineState.Finished,
        ExitCode = process.ExitCode
      };

    }

    public async Task<CommandlineResult> RunAsync()
    {
      try
      {
        return await RunInternAsync();
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
