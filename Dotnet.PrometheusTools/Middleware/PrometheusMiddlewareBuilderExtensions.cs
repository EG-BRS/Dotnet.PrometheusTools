using Microsoft.AspNetCore.Builder;
using System;

namespace PrometheusTools.Middleware
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
