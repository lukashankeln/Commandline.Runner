using System;
using System.Text;

namespace Hankeln.Commandline.Runner
{
  public sealed class CommandlineOptions
  {
    /// <summary>
    /// If set the Commandline will wait for the Process to exit
    /// </summary>
    public bool WaitForExit { get; set; }

    /// <summary>
    /// If set the Std and Error out are logged
    /// </summary>
    public bool WriteLog { get; set; }

    /// <summary>
    /// If set and <seealso cref="WaitForExit"/> is set the started Process is killed after specified time
    /// </summary>
    /// <remarks>Only exact to 500ms</remarks>
    public TimeSpan? Timeout { get; set; }

    /// <summary>
    /// Std-Out and Err-Out Encoding, if not set, defaults to UTF8
    /// </summary>
    public Encoding? Encoding { get; set; }
  }
}
