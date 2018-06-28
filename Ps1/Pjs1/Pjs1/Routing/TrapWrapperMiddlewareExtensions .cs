using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pjs1.Main.Routing
{
    public static class TrapWrapperMiddlewareExtensions
    {
        public static IApplicationBuilder UseTrapWrapperMiddleware(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.Use(next =>
            {
               // var re=  new TrapWrapperMiddleware(next);
                return context =>
                {
                    Task SimpleNext() => next(context);
                    return middleware(context, SimpleNext);
                };
            });
        }

        private static Task middleware(HttpContext context, Func<Task> simpleNext)
        {
            throw new NotImplementedException();
        }
    }


    public class TrapWrapperMiddleware
    {
        //private readonly ILogger _logger; // TODO: implement logger
        private readonly RequestDelegate _next;

        private static readonly HashSet<string> _ignored = new HashSet<string>
        {
            "/url/reject/01",
            "/url/reject/02",
            "/url/reject/03"
        };

        private static readonly HashSet<string> _tracked = new HashSet<string>
        {
            "/url/log/01",
            "/url/log/02",
            "/url/log/03"
        };

        public TrapWrapperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var contextPath = context.Request.Path;

            if (contextPath.HasValue && _ignored.Contains(contextPath.Value))
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            }

            if (contextPath.HasValue && _tracked.Contains(contextPath.Value))
            {
                using (var injectedRequestStream = new MemoryStream())
                {
                    var requestLog = string.Empty;
                    if (context.Request.Method == "POST" || context.Request.Method == "PUT")
                    {
                        await context.Request.Body.CopyToAsync(injectedRequestStream);
                        using (var memoryStream = new MemoryStream(injectedRequestStream.ToArray()))
                        {
                            using (var bodyReader = new StreamReader(memoryStream))
                            {
                                var bodyAsText = await bodyReader.ReadToEndAsync();
                                if (string.IsNullOrWhiteSpace(bodyAsText) == false)
                                {
                                    requestLog += $", Body : {bodyAsText}";
                                }
                            }
                        }
                        injectedRequestStream.Seek(0, SeekOrigin.Begin);
                        context.Request.Body = injectedRequestStream;
                    }
                    //TODO : log >> requestLog << 

                }

            }

            await _next.Invoke(context);
        }
    }
}
