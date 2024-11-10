using System;

public class ConfigService : IConfigService
{
    public string GetBaseHostname()
    {
        return Environment.GetEnvironmentVariable("HOSTNAME") ?? "https://url.ilmhub.uz";
    }
}
