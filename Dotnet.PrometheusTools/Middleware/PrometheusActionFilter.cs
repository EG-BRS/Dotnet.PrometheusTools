using Microsoft.AspNetCore.Mvc.Filters;
using Prometheus.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PrometheusTools.Middleware
{
    public class PrometheusActionFilter : IActionFilter
    {
        private readonly PrometheusMiddlewareOptions _options;
        private Summary _summary;

        public PrometheusActionFilter(PrometheusMiddlewareOptions options)
        {
            _options = options;
            var test = new string[] { "method", "controller", "action", "statuscode" };
            _summary = Metrics.CreateSummary("http_response_time_milliseconds_v2", "Request duration in milliseconds", test);
        }

         public void OnActionExecuting(ActionExecutingContext context)
        {
            var route = context.HttpContext.Request.Path.ToString();

            foreach (var i in _options.ExcludeRoutes)
            {
                if (route.Contains(i))
                {
                    return;
                }
            }

            var watch = Stopwatch.StartNew();

            var action = context.RouteData.Values["action"].ToString();
            var controller = context.RouteData.Values["controller"].ToString();
            var method = context.HttpContext.Request.Method;

            context.HttpContext.Items.Add("stopwatch", watch);
            context.HttpContext.Items.Add("action", action);
            context.HttpContext.Items.Add("controller", controller);
            context.HttpContext.Items.Add("method", method);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var watch = (Stopwatch) context.HttpContext.Items["stopwatch"];

            if (watch == null) return; 

            var action = context.RouteData.Values["action"].ToString();
            var controller = context.RouteData.Values["controller"].ToString();
            var method = context.HttpContext.Request.Method;
            var statuscode = context.HttpContext.Response.StatusCode.ToString();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            if (elapsedMs > 0)
            {
                double ms = elapsedMs;
                _summary.Labels(method, controller, action, statuscode).Observe(ms);
            }
        }
    }
}
