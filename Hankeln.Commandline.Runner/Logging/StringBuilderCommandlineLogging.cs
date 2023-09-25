using System;
using System.Text;

namespace Hankeln.Commandline.Runner.Logging
{
  public class StringBuilderCommandlineLogging : ICommandlineLogging
  {
    public StringBuilderCommandlineLogging()
    {
      ErrOutBuilder = new StringBuilder();
      StdOutBuilder = new StringBuilder();
    }

    public StringBuilder ErrOutBuilder { get; }
    public StringBuilder StdOutBuilder { get; }

    public void ErrOut(string message)
    {
      ErrOutBuilder.AppendLine(message);
    }

    public void Message(string message)
    {
      Console.WriteLine(message);
    }

    public void StdOut(string message)
    {
      StdOutBuilder.AppendLine(message);
    }


    internal void Deconstruct(out string stdOut, out string errOut)
    {
      stdOut = StdOutBuilder.ToString();
      errOut = ErrOutBuilder.ToString();
    }
  }
}
