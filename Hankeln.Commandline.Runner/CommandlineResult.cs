namespace Hankeln.Commandline.Runner
{
  public sealed class CommandlineResult
  {
    public const int Faulted = -1324697654;
    public CommandlineState State { get; internal set; }
    public int? ExitCode { get; internal set; }

    public enum CommandlineState
    {
      Running, Finished, Timeout, Faulted
    }

    public bool Success => ExitCode.HasValue && ExitCode.Value == 0 && State == CommandlineState.Finished;
  }
}
