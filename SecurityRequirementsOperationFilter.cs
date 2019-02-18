using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AuthBug
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter)
                .Any(filter => filter is AuthorizeFilter);
            var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter)
                .Any(filter => filter is IAllowAnonymousFilter);


            if (isAuthorized && !allowAnonymous)
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                operation.Responses.Add("403", new Response { Description = "Forbidden" });

                if (operation.Security == null)
                {
                    operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
                }

                var authSchemes = filterPipeline.Select(filterInfo => filterInfo.Filter).OfType<AuthorizeFilter>()
                    .SelectMany(filter => filter.Policy.AuthenticationSchemes);

                var scheme = authSchemes.Any(s => s == ApiKeyDefaults.AuthenticationScheme)
                    ? ApiKeyDefaults.AuthenticationScheme
                    : JwtBearerDefaults.AuthenticationScheme;

                operation.Security.Add(new Dictionary<string, IEnumerable<string>> {
                    {scheme, new string[0]}
                });
            }
        }
    }
}
