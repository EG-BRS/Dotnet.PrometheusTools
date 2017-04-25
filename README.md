# Prometheus.Tools
unofficial .NET Core tools for prometheus metrics

Suggestions for improvements or additional tools are most welcome!

# Installation
Install the Prometheus.Tools nuget package
```
Install-Package Prometheus.Tools
```

# Usage

## Middleware
The middleware is designed to enable easy metrics logging of request durations.

The package is currently depending on the Prometheus.Client nuget package. Information on how to use it can be found here: [https://github.com/phnx47/Prometheus.Client](https://github.com/phnx47/Prometheus.Client)

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
{
    var options = new PrometheusOptions();
    options.MapPath = "metrics";
    options.Collectors.Add(new DotNetStatsCollector());
    app.UsePrometheusServer(options);

    app.UsePrometheusMiddleware(c =>
    {
        c.ExcludeRoutes.Add("swagger");
    });
}
```

In the above example any routes with `swagger` will be excluded from the metrics and not be logged. If you do not want to exclude any routes replace it with the below line instead:

```csharp
    app.UsePrometheusMiddleware();
```

To exclude several routes:
```csharp
    app.UsePrometheusMiddleware(c =>
    {
        c.ExcludeRoutes.AddRange(new[] { "swagger", "Values" });
    });
```

Excepted metrics out
```
# HELP http_response_time_milliseconds Request duration in milliseconds
# TYPE http_response_time_milliseconds SUMMARY
http_response_time_milliseconds_sum{method="GET",route="/quality/api/values",statuscode="200"} 620
http_response_time_milliseconds_count{method="GET",route="/quality/api/values",statuscode="200"} 25
http_response_time_milliseconds{method="GET",route="/quality/api/values",statuscode="200",quantile="0.5"} 17
http_response_time_milliseconds{method="GET",route="/quality/api/values",statuscode="200",quantile="0.9"} 22
http_response_time_milliseconds{method="GET",route="/quality/api/values",statuscode="200",quantile="0.99"} 22
```
