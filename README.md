# Serilog.Enrichers.CorrelationId

Enriches Serilog events with a correlation ID for tracking requests.

[![Build status](https://ci.appveyor.com/api/projects/status/c280e547sj758qfc/branch/master?svg=true)](https://ci.appveyor.com/project/ejcoyle88/serilog-enrichers-correlation-id/branch/master)
[![Coverage Status](https://coveralls.io/repos/github/ekmsystems/serilog-enrichers-correlation-id/badge.svg?branch=master)](https://coveralls.io/github/ekmsystems/serilog-enrichers-correlation-id?branch=master)
[![NuGet](http://img.shields.io/nuget/v/Serilog.Enrichers.CorrelationId.svg?style=flat)](https://www.nuget.org/packages/Serilog.Enrichers.CorrelationId/)

To use the enricher, first install the NuGet package:

```powershell
Install-Package Serilog.Enrichers.CorrelationId
```

Then, apply the enricher to your `LoggerConfiguration`:

```csharp
Log.Logger = new LoggerConfiguration()
    .Enrich.WithCorrelationId()
    // ...other configuration...
    .CreateLogger();
```

__IT IS CRUCIAL FOR THIS ENRICHER TO WORK TO ENABLE ACCESS TO HTTP CONTEXT__
```
Startup.cs

	public void ConfigureServices(IServiceCollection services)
	{
        ..
        	services.AddHttpContextAccessor();
        ..
        }
```


The `WithCorrelationId()` enricher will add a `CorrelationId` property to produced events.

### Included enrichers

The package includes:

 * `WithCorrelationId()` - adds a `CorrelationId` to track logs for the current web request.
 * `WithCorrelationIdHeader(headerKey)` - adds a `CorrelationId` extracted from the current request header (or created if one does not exist).
