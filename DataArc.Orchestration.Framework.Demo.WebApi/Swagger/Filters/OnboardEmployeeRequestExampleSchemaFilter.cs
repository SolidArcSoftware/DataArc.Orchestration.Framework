using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Dtos;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace DataArc.Orchestration.Framework.Demo.WebApi.Swagger
{
    public sealed class OnboardEmployeeRequestExampleSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type != typeof(OnboardEmployeeRequestDto))
                return;

            schema.Example = new OpenApiObject
            {
                ["employeeId"] = new OpenApiInteger(1),
                ["annualSalary"] = new OpenApiDouble(85000),
                ["currencyCode"] = new OpenApiString("USD"),
                ["reason"] = new OpenApiString("Demo employee onboarding")
            };
        }
    }
}