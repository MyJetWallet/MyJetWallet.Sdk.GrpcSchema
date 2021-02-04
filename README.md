![Nuget version](https://img.shields.io/nuget/v/MyJetWallet.Sdk.GrpcSchema?label=MyJetWallet.Sdk.GrpcSchema&style=social)

## Usage

```csharp
public class Startup
{
    app.UseEndpoints(endpoints =>
    {
        // register MyGrpcCodeFirstService and register a contract of the service - IMyGrpcCodeFirstService
        endpoints.MapGrpcSchema<MyGrpcCodeFirstService, IMyGrpcCodeFirstService>();

        endpoints.MapGrpcSchemaRegistry();    
    };
}
```
