using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Hankeln.Commandline.Runner.CommandlineResult;

namespace Hankeln.Commandline.Runner.Test
{
  public class ResultTest
  {
    [Theory]
    [InlineData(true, 0)]
    [InlineData(false, 1)]
    [InlineData(false, -1)]
    public void Should_Evaluate_Result_Object_By_ExitCode(bool expected, int exitcode)
    {
      var res = new CommandlineResult() { ExitCode = exitcode, State = CommandlineState.Finished };

      Assert.Equal(expected, res.Success);
    }

    [Theory]
    [InlineData(true, CommandlineState.Finished)]
    [InlineData(false, CommandlineState.Faulted)]
    [InlineData(false, CommandlineState.Running)]
    [InlineData(false, CommandlineState.Timeout)]
    public void Should_Evaluate_Result_Object_By_State(bool expected, CommandlineResult.CommandlineState state)
    {
      var res = new CommandlineResult() { State = state, ExitCode = 0 };

      Assert.Equal(expected, res.Success);
    }
  }
}
