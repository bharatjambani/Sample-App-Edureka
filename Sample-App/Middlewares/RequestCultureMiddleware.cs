using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Sample_App.Middlewares
{
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate _nextMiddleware;
        public RequestCultureMiddleware(RequestDelegate nextMiddleware)
        {

            _nextMiddleware = nextMiddleware;

        }

        //Read QueryString and Set the culture according to the string passed in the URL
        //InvokeAsync is automatically called
        public async Task InvokeAsync(HttpContext context)
        {
            var cultureQuery = context.Request.Query["Culture"];
            if (!String.IsNullOrWhiteSpace(cultureQuery))
            {
                var culture = new CultureInfo(cultureQuery);
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
                
            }
            await _nextMiddleware(context); // Call next middleware

        }
    }
}
