[https://github.com/dotnet/aspnetcore/issues/36452](https://github.com/dotnet/aspnetcore/issues/36452)


In my project, I created a custom configuration provider, which is mainly used to process `.txt` files.

After that, I added a custom configuration provider through the following code and found that the provider's `Load` method was **called multiple times**. 

The following code is added through `WebApplicationBuilder.Configuration`.

```cs
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
```

![image](https://user-images.githubusercontent.com/22766122/133021433-690fa848-448b-4615-87b4-bdee34f55c9d.png)

---

However, if I use the `Host.CreateDefaultBuilder().ConfigureAppConfiguration()` method to add a custom configuration provider, the `Load` method will **only be called once**, which is what I expect.

```cs
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
```

![image](https://user-images.githubusercontent.com/22766122/133021396-814bced0-58d1-4345-9120-0f8902187f5a.png)

---

🟡 Test Code：[https://github.com/MonkSoul/net6-customize-configuration-souce/tree/master/ConfigurationTests](https://github.com/MonkSoul/net6-customize-configuration-souce/tree/master/ConfigurationTests)
