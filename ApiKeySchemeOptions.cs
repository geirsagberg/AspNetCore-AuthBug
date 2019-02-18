using System;
using Microsoft.AspNetCore.Authentication;

namespace AuthBug
{
    public class ApiKeySchemeOptions : AuthenticationSchemeOptions
    {
        public Guid ApiKey { get; set; }
    }
}
