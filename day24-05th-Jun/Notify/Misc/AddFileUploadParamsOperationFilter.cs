namespace Notify.Misc

{

    using Microsoft.OpenApi.Models;

    using Swashbuckle.AspNetCore.SwaggerGen;

    using System.Linq;
 
    public class AddFileUploadParamsOperationFilter : IOperationFilter

    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)

        {

            var fileParams = context.MethodInfo.GetParameters()

                .Where(p => p.ParameterType == typeof(Microsoft.AspNetCore.Http.IFormFile));
 
            if (!fileParams.Any())

                return;
 
            operation.RequestBody = new OpenApiRequestBody

            {

                Content =

                {

                    ["multipart/form-data"] = new OpenApiMediaType

                    {

                        Schema = new OpenApiSchema

                        {

                            Type = "object",

                            Properties =

                            {

                                ["file"] = new OpenApiSchema

                                {

                                    Type = "string",

                                    Format = "binary"

                                }

                            },

                            Required = new HashSet<string> { "file" }

                        }

                    }

                }

            };

        }

    }

}
 