using Hankeln.Commandline.Runner.Logging;
using System;
using System.Collections.Generic;

namespace Hankeln.Commandline.Runner
{
  /// <summary>
  /// A Factory to easily create a new <see cref="Commandline"/>
  /// </summary>
  public class CommandlineFactory
  {
    /// <summary> The arguments to be set in <see cref="Commandline.Arguments"/> </summary>
    protected List<string> Arguments { get; }

    /// <summary> The file to be set in <see cref="Commandline.FileName"/> </summary>
    protected string FileName { get; }

    /// <summary> The options to be set in <see cref="Commandline.Options"/> </summary>
    protected CommandlineOptions Options { get; }

    /// <summary> The logger to be set in <see cref="Commandline.Logger"/> </summary>
    protected ICommandlineLogging Logger { get; private set; }

    private CommandlineFactory(string file)
    {
      Arguments = new List<string>();
      FileName = file;
      Options = new CommandlineOptions();
      Logger = NoopCommandlineLogging.Instance;
    }

    /// <summary>
    /// Creates an new factory to create a new Commandline for the given file
    /// </summary>
    /// <param name="file">The file to be executed by the commandline</param>
    /// <returns>A new factory for creating the commandline</returns>
    public static CommandlineFactory Create(string file) => new(file);


    /// <summary>
    /// Adds a new argument to the list of arguments for the process
    /// </summary>
    /// <param name="argument">The argument given to the process</param>
    /// <returns>This factory, for fluent use</returns>
    public CommandlineFactory AddArgument(string argument)
    {
      Arguments.Add(argument);
      return this;
    }

    /// <summary>
    /// Sets the logger to be used
    /// </summary>
    /// <param name="logger">The logger to be used</param>
    /// <returns>This factory, for fluent use</returns>
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
    /// <returns>This factory, for fluent use</returns>
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
