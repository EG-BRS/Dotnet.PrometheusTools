using Microsoft.AspNetCore.Builder;
using System;

namespace dotnet_prometheus_middleware
{
    public static class PrometheusMiddlewareBuilderExtensions
    {
        public static IApplicationBuilder UsePrometheusMiddleware(
            this IApplicationBuilder app,
            Action<PrometheusMiddlewareOptions> setupAction = null)
        {
            var options = new PrometheusMiddlewareOptions();
            setupAction?.Invoke(options);

            return app.UseMiddleware<PrometheusMiddlware>(options);
        }
    }
}
