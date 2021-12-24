using Microsoft.AspNetCore.Builder;
using Sample_App.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample_App.Extensions
{
    //Extension methods rules
    //Class and mthod should be static
    public static class RequestMiddlewareExtensions
    {
        //Whenever someone calls UseRequestCulture he should call RequestCultureMiddleware
        public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCultureMiddleware>();
        }
    }
}
