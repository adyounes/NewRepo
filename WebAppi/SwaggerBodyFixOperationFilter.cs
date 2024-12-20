﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class SwaggerBodyFixOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var parameter in operation.Parameters)
        {
            parameter.Name = char.ToLowerInvariant(parameter.Name[0]) + parameter.Name.Substring(1);
        }
    }
}
