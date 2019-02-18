using System;
using Microsoft.Extensions.Options;

namespace AuthBug
{
    public class ApiKeyPostConfigureOptions : IPostConfigureOptions<ApiKeySchemeOptions>
    {
        public void PostConfigure(string name, ApiKeySchemeOptions options)
        {
            if (options.ApiKey == default)
            {
                throw new InvalidOperationException("ApiKey must be provided in ApiKeyOptions");
            }
        }
    }
}
