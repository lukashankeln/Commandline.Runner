using Hankeln.Commandline.Runner.Logging;
using System;
using System.Collections.Generic;

namespace Hankeln.Commandline.Runner
{
  public class CommandlineFactory
  {
    protected List<string> Arguments { get; }
    protected string FileName { get; }
    protected CommandlineOptions Options { get; }
    protected ICommandlineLogging Logger { get; private set; }

    private CommandlineFactory(string file)
    {
      Arguments = [];
      FileName = file;
      Options = new();
      Logger = NoopCommandlineLogging.Instance;
    }

    public static CommandlineFactory Create(string file) => new(file);


    public CommandlineFactory AddArgument(string argument)
    {
      Arguments.Add(argument);
      return this;
    }

    /// <summary>
    /// Sets the logger to be used
    /// </summary>
    /// <param name="logger">The logger to be used</param>
    /// <returns>This CommandlineFactory</returns>
    /// <remarks>This sets <seealso cref="CommandlineOptions.WriteLog"/> to <![CDATA[true]]></remarks>
    public CommandlineFactory WithLogger(ICommandlineLogging logger)
    {
      Logger = logger;
      Options.WriteLog = true;
      return this;
    }

    /// <summary>
    /// This sets the timeout to be used
    /// </summary>
    /// <param name="timeout">The timeout to be used</param>
    /// <returns>This CommandlineFactory</returns>
    /// <remarks>This sets <seealso cref="CommandlineOptions.Timeout"/> to the timeout specified
    /// and also <seealso cref="CommandlineOptions.WaitForExit"/> to <![CDATA[true]]></remarks>
    public CommandlineFactory WithTimeout(TimeSpan timeout)
    {
      Options.Timeout = timeout;
      Options.WaitForExit = true;
      return this;
    }

    /// <summary>
    /// Creates the Commandline, that can be executed
    /// </summary>
    /// <returns>Commandline that can be executed</returns>
    public Commandline Build()
     => new(FileName, Arguments, Options, Logger);

    /// <summary>
    /// Creates the Commandline, that can be executed, see <seealso cref="Build()"/>
    /// </summary>
    /// <param name="options">An action where the <seealso cref="CommandlineOptions"/> can be changed</param>
    /// <returns>Commandline that can be executed</returns>
    public Commandline Build(Action<CommandlineOptions> options)
    {
      options(Options);
      return Build();
    }

  }
}
