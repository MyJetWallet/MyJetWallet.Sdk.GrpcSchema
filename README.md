MyJetWallet.Sdk.GrpcMetrics

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
