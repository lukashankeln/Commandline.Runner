namespace Hankeln.Commandline.Runner.Logging
{
  public sealed class NoopCommandlineLogging : ICommandlineLogging
  {
    public static ICommandlineLogging Instance { get; } = new NoopCommandlineLogging();

    public void ErrOut(string message)
    {
    }

    public void Message(string message)
    {
    }

    public void StdOut(string message)
    {
    }
  }
}
