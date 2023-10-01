namespace Hankeln.Commandline.Runner
{
  /// <summary>
  /// A result Object defining the result for the started process
  /// </summary>
  public sealed class CommandlineResult
  {
    /// <summary>
    /// Exitcode to be set, if the Process could not bestarted
    /// </summary>
    public const int Faulted = -1324697654;

    /// <summary>
    /// The state the process currently has
    /// </summary>
    public CommandlineState State { get; internal set; }

    /// <summary>
    /// The Exitcode set by the process, only set if Process has endet
    /// </summary>
    public int? ExitCode { get; internal set; }

    /// <summary>
    /// The possibly states a Commandline can have
    /// </summary>
    public enum CommandlineState
    {
      /// <summary> The process is still running </summary>
      Running,
      /// <summary> The process has finished </summary>
      Finished,
      /// <summary> The process was killed because of timeout </summary>
      Timeout,
      /// <summary> The process could not be started </summary>
      Faulted
    }

    /// <summary>
    /// Indicates, if the process executed successfully
    /// </summary>
    public bool Success => ExitCode.HasValue && ExitCode.Value == 0 && State == CommandlineState.Finished;
  }
}
