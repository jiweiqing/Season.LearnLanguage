using Learning.Domain;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Learning.AspNetCore
{
    public static class ExceptionHandlerExtensions
    {
        public static void UseAppExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseExceptionHandler(options =>
            {
                options.Run(async context =>
                {
                    // HttpStatusCode
                    int statusCode = StatusCodes.Status500InternalServerError;
                    var errorResponse = new ApiErrorResponse();

                    var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
                    if (exception != null)
                    {
                        // unauthorize
                        if (exception is UnauthorizedAccessException unAuthEx)
                        {
                            statusCode = (int)HttpStatusCode.Forbidden;
                            errorResponse.Code = statusCode.ToString();
                            errorResponse.Message = unAuthEx.Message;
                        }
                        else
                        {
                            if (exception is BusinessException businessException)
                            {
                                statusCode = (int)HttpStatusCode.BadRequest;
                                errorResponse.Code = businessException.Code;
                                errorResponse.Message = businessException.Message;
                            }
                            else if (environment.IsDevelopment())
                            {
                                errorResponse.Message = exception.Message;
                            }
                            else
                            {
                                // localization
                                // var localizer = context.RequestServices.GetService<IStringLocalizer<object>>();
                                errorResponse.Message = "Internal server error";
                            }
                        }
                    }

                    context.Response.StatusCode = statusCode;
                    context.Response.ContentType = "application/json; charset=utf-8";

                    // 默认 application/json; charset=utf-8
                    await context.Response.WriteAsJsonAsync(errorResponse);
                });
            });
        }


        /// <summary>
        /// ConfigureApiBehaviorOptions
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureApiBehaviorOptions(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ModelResponseFactory;
            });
        }


        /// <summary>
        /// 配置模型校验错误返回格式
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private static IActionResult ModelResponseFactory(ActionContext actionContext)
        {
            var errorResponse = new ApiErrorResponse()
            {
                Code = StatusCodes.Status422UnprocessableEntity.ToString(),
                Message = "One or more validation errors occurred"
            };

            var validationErrors = new List<ValidationErrorInfo>();

            foreach (var item in actionContext.ModelState)
            {
                var errorInfo = new ValidationErrorInfo
                {
                    Member = item.Key,
                    // item.Value.Errors.Count
                    // errorInfo.Messages = new string[item.Value.Errors.Count];

                    Messages = item.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                };

                validationErrors.Add(errorInfo);
            }

            errorResponse.ValidationErrors = validationErrors.ToArray();

            return new UnprocessableEntityObjectResult(errorResponse)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            };
        }
    }
}
