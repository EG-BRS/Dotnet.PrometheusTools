using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PrometheusTools.Middleware
{
    public class PrometheusFilter : IActionFilter
    {

         public void OnActionExecuting(ActionExecutingContext context)
        {
            var watch = Stopwatch.StartNew();

            context.HttpContext.Items.Add("stopwatch", watch);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var watch = (Stopwatch) context.HttpContext.Items["stopwatch"];

            var method = context.RouteData.Values["method"];
            var method2 = context.RouteData.Values["action"];
            var method3 = context.RouteData.Values["controller"];
            var method4 = context.RouteData.Values["method"];
            //var statusCode = context.Response.StatusCode.ToString();


            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            if (elapsedMs > 0)
            {
                double ms = elapsedMs;
                //_summary.Labels(method, route, statusCode).Observe(ms);
            }
        }
    }
}
