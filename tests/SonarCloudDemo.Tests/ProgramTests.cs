using Xunit;

namespace SonarCloudDemo.Tests;

public class ProgramTests
{
    [Fact]
    public void Main_RunsWithoutThrowing()
    {
        var entryPoint = typeof(AppRunner).Assembly.EntryPoint;
        Assert.NotNull(entryPoint);

        entryPoint.Invoke(null, [Array.Empty<string>()]);
    }
}
