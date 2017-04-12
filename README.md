# Serilog.Enrichers.CorrelationId

Enriches Serilog events with a correlation ID for tracking requests.

[![Build status](https://ci.appveyor.com/api/projects/status/2pmyw7ll2078fskd?svg=true)](https://ci.appveyor.com/project/mrstebo/serilog-enrichers-correlation-id)
[![NuGet Version](http://img.shields.io/nuget/v/Serilog.Enrichers.CorrelationId.svg?style=flat)](https://www.nuget.org/packages/Serilog.Enrichers.CorrelationId/)

To use the enricher, first install the NuGet package:

```powershell
Install-Package Serilog.Enrichers.CorrelationId
```

Then, apply the enricher to you `LoggerConfiguration`:

```csharp
Log.Logger = new LoggerConfiguration()
    .Enrich.WithCorrelationId()
    // ...other configuration...
    .CreateLogger();
```

The `WithCorrelationId()` enricher will add a `CorrelationId` property to produced events.

### Included enrichers

The package includes:

 * `WithCorrelationId()` - adds a `CorrelationId` to track logs for the current web request.
 * `WithCorrelationIdHeader(headerKey)` - adds a `CorrelationId` extracted from the current request header (or created if one does not exist).
