using System.Collections.Generic;

namespace PrometheusTools.Middleware
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
