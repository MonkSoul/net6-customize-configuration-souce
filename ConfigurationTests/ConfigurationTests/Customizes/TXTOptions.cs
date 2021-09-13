using Xunit.Abstractions;

namespace ConfigurationTests;

public class TXTOptions
{
    public string Path { get; set; }
    public ITestOutputHelper Output { get; set; }
}