using Microsoft.Extensions.Configuration;
using System;

namespace ConfigurationTests;

public static class TXTConfigurationExtensions
{
    public static IConfigurationBuilder AddTXTConfiguration(this IConfigurationBuilder builder, Action<TXTOptions> optionsAction)
    {
        return builder.Add(new TXTConfigurationSource(optionsAction));
    }
}