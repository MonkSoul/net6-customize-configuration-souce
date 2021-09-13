using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConfigurationTests;

public class TXTConfigurationProvider : ConfigurationProvider
{
    public TXTConfigurationProvider(Action<TXTOptions> optionsAction)
    {
        OptionsAction = optionsAction;
    }

    Action<TXTOptions> OptionsAction { get; }

    public override void Load()
    {
        var options = new TXTOptions();
        OptionsAction(options);

        options.Output.WriteLine("TXTConfigurationSource.Build() is called.");

        var dic = Path.GetDirectoryName(options.Path);
        var fileinfo = new PhysicalFileProvider(dic).GetFileInfo(Path.GetFileName(options.Path));

        using var stream = fileinfo.CreateReadStream();
        var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        using var reader = new StreamReader(stream);
        while (reader.Peek() > -1)
        {
            var lineText = reader.ReadLine()!;
            if (string.IsNullOrWhiteSpace(lineText.Trim()))
            {
                continue;
            }

            var splits = lineText.Split('=', StringSplitOptions.RemoveEmptyEntries);
            dictionary[splits[0]] = splits[1];
        }

        base.Data = dictionary;
    }
}