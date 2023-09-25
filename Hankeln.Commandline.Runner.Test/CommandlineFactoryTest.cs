using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hankeln.Commandline.Runner.Test
{
  public class CommandlineFactoryTest
  {
    [Fact]
    public void Should_Build_Basic_Commandline()
    {
      var cmd = CommandlineFactory.Create("test").Build();

      Assert.Multiple(() => Assert.NotNull(cmd), 
                      () => Assert.Equal("test", cmd.FileName), 
                      () => Assert.Empty(cmd.Arguments), 
                      () => Assert.NotNull(cmd.Options),
                      () => Assert.Null(cmd.Options.Timeout),
                      () => Assert.False(cmd.Options.WaitForExit),
                      () => Assert.False(cmd.Options.WriteLog),
                      () => Assert.Null(cmd.Options.Encoding));
    }

    [Fact]
    public void Should_Build_Basic_Commandline_With_Methods()
    {
      var cmd = CommandlineFactory.Create("test")
                                  .WithTimeout(TimeSpan.FromHours(1))
                                  .Build();

      Assert.Multiple(() => Assert.NotNull(cmd),
                      () => Assert.Equal("test", cmd.FileName),
                      () => Assert.Empty(cmd.Arguments),
                      () => Assert.NotNull(cmd.Options),
                      () => Assert.NotNull(cmd.Options.Timeout),
                      () => Assert.True(cmd.Options.WaitForExit),
                      () => Assert.False(cmd.Options.WriteLog),
                      () => Assert.Null(cmd.Options.Encoding));
    }

    [Fact]
    public void Should_Build_Commandline_With_Arguments()
    {
      var cmd = CommandlineFactory.Create("test").AddArgument("--foo").Build();
      
      Assert.Multiple(() => Assert.NotNull(cmd),
                      () => Assert.Equal("test", cmd.FileName),
                      () => Assert.Single(cmd.Arguments),
                      () => Assert.NotNull(cmd.Options),
                      () => Assert.Null(cmd.Options.Timeout),
                      () => Assert.False(cmd.Options.WaitForExit),
                      () => Assert.False(cmd.Options.WriteLog),
                      () => Assert.Null(cmd.Options.Encoding));
    }

    [Fact]
    public void Should_Build_Commandline_With_Arguments_And_Options()
    {
      var cmd = CommandlineFactory.Create("test").AddArgument("--foo").Build(options =>
      {
        options.WaitForExit = true;
        options.WriteLog = true;
      });

      Assert.Multiple(() => Assert.NotNull(cmd),
                      () => Assert.Equal("test", cmd.FileName),
                      () => Assert.Single(cmd.Arguments),
                      () => Assert.NotNull(cmd.Options),
                      () => Assert.Null(cmd.Options.Timeout),
                      () => Assert.True(cmd.Options.WaitForExit),
                      () => Assert.True(cmd.Options.WriteLog),
                      () => Assert.Null(cmd.Options.Encoding));
    }
  }
}
