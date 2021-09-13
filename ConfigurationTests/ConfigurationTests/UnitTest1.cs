using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace ConfigurationTests;
public class UnitTest1
{
    protected readonly ITestOutputHelper Output;

    public UnitTest1(ITestOutputHelper tempOutput)
    {
        Output = tempOutput;
    }

    /// <summary>
    /// TXTConfigurationSource.Build() call once
    /// </summary>
    [Fact]
    public void TestHostBuilder()
    {
        var builder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddTXTConfiguration(options =>
                {
                    options.Output = Output;
                    options.Path = Path.Combine(AppContext.BaseDirectory, "file.txt");
                });
            });

        var app = builder.Build();
        var services = app.Services;

        var configuration = services.GetRequiredService<IConfiguration>();
        Assert.Equal("VALUE", configuration["TXT"]);
    }

    /// <summary>
    /// TXTConfigurationSource.Build() call multiple times
    /// </summary>
    [Fact]
    public void TestWebApplicationBuilder()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.AddTXTConfiguration(options =>
        {
            options.Output = Output;
            options.Path = Path.Combine(AppContext.BaseDirectory, "file.txt");
        });

        using var app = builder.Build();
        var services = app.Services;

        var configuration = services.GetRequiredService<IConfiguration>();
        Assert.Equal("VALUE", configuration["TXT"]);
    }
}