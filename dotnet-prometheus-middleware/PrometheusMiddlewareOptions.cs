using System.Collections.Generic;

namespace dotnet_prometheus_middleware
{
    public class PrometheusMiddlewareOptions
    {
        public PrometheusMiddlewareOptions()
        {
            ExcludeRoutes = new List<string>();
        }

        public List<string> ExcludeRoutes { get; set; }
    }
}
