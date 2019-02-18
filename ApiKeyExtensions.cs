using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AuthBug
{
    public static class ApiKeyExtensions
    {

        public static AuthenticationBuilder AddApiKey(this AuthenticationBuilder builder,
            Action<ApiKeySchemeOptions> configure)
        {
            builder.Services.AddSingleton<IPostConfigureOptions<ApiKeySchemeOptions>, ApiKeyPostConfigureOptions>();
            return builder.AddScheme<ApiKeySchemeOptions, ApiKeyAuthenticationHandler>(
                ApiKeyDefaults.AuthenticationScheme,
                configure);
        }
    }
}
