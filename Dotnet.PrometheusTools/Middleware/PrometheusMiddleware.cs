using Microsoft.AspNetCore.Http;
using Prometheus.Client;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace PrometheusTools.Middleware
{
    public class PrometheusMiddlware
    {
        private readonly RequestDelegate _next;
        private readonly PrometheusMiddlewareOptions _options;
        private Summary _summary;

        public PrometheusMiddlware(RequestDelegate next, PrometheusMiddlewareOptions options)
        {
            _next = next;
            _options = options;
            var test = new string[] { "method", "route", "statuscode" };
            _summary = Metrics.CreateSummary("http_response_time_milliseconds", "Request duration in milliseconds", test);
        }

        public async Task Invoke(HttpContext context)
        {
            var route = context.Request.Path.ToString();

            foreach (var i in _options.ExcludeRoutes)
            {
                if (route.Contains(i))
                {
                    await _next.Invoke(context);
                    return;
                }
            }
            
            var watch = Stopwatch.StartNew();
            await _next.Invoke(context);

            var method = context.Request.Method.ToString();
            var statusCode = context.Response.StatusCode.ToString();            

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            if (elapsedMs > 0)
            {
                double ms = elapsedMs;
                _summary.Labels(method, route, statusCode).Observe(ms);
            }
        }
    }
}
