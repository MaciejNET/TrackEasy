using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace TrackEasy.Shared.Infrastructure;

public static class KeyVaultExtensions
{
    public static IConfigurationBuilder AddKeyVault(this IConfigurationBuilder builder)
    {
        var config = builder.Build();
        var vaultUrl = config["KeyVault:Url"];

        if (string.IsNullOrWhiteSpace(vaultUrl))
            return builder;

        builder.AddAzureKeyVault(new Uri(vaultUrl), new DefaultAzureCredential());
        return builder;
    }
}