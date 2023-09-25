namespace Hankeln.Commandline.Runner.Logging
{
  public interface ICommandlineLogging
  {
    /// <summary>
    /// Writes Std-Out
    /// </summary>
    /// <param name="message">Message from Std-Out</param>
    void StdOut(string message);

    /// <summary>
    /// Writes Err-Out
    /// </summary>
    /// <param name="message">Message from Err-Out</param>
    void ErrOut(string message);

    /// <summary>
    /// Writes Messages from this Library
    /// </summary>
    /// <param name="message">Message from Library</param>
    void Message(string message);
  }
}
