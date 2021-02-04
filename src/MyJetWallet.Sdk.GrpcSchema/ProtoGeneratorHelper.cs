using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ProtoBuf.Grpc.Reflection;

namespace MyJetWallet.Sdk.GrpcSchema
{
    public static class ProtoGeneratorHelper
    {
        private static readonly Dictionary<string, string> Map = new Dictionary<string, string>();

        public static void MapGrpcSchema<TService, TSchema>(this IEndpointRouteBuilder endpoints) where TService : class, TSchema
        {
            var service = typeof(TSchema);
            var generator = new SchemaGenerator();
            var schema = generator.GetSchema(service);
            Map[service.Name] = schema;

            endpoints.Map($"/grpc/{service.Name}.proto",
                async context =>
                {
                    await context.Response.WriteAsync(schema);
                });

            endpoints.MapGrpcService<TService>();

        }

        public static void MapGrpcSchemaRegistry(this IEndpointRouteBuilder endpoints)
        {
            endpoints.Map("/grpc",
                async context =>
                {
                    await context.Response.WriteAsync(GetListOfServices());
                });

            endpoints.Map("/grpc/protobuf-net/bcl.proto", async context =>
            {
                await context.Response.WriteAsync(GetBcl());
            });
        }

        private static string GetListOfServices()
        {
            var builder = new StringBuilder();

            builder.AppendLine("<html>");
            builder.AppendLine("<body>");

            builder.AppendLine("<p>List of proto files</p>");

            builder.AppendLine("<ul>");

            builder.AppendLine("<li><a href='/grpc/protobuf-net/bcl.proto'>/grpc/protobuf-net/bcl.proto</a><br></li>");

            foreach (var key in Map.Keys)
            {
                builder.AppendLine($"<li><a href='/grpc/{key}.proto'>/grpc/{key}.proto</a><br></li>");
            }

            builder.AppendLine("</ul>");

            builder.AppendLine("</body>");
            builder.AppendLine("</html>");

            return builder.ToString();
        }

        private static string GetBcl()
        {
            return
                @"// The types in here indicate how protobuf-net represents certain types when using protobuf-net specific
// library features. Note that it is not *required* to use any of these types, and cross-platform code
// should usually avoid them completely (ideally starting from a .proto schema)

// Some of these are ugly, sorry. The TimeSpan / DateTime dates here pre-date the introduction of Timestamp
// and Duration, and the ""well known"" types should be preferred when possible. Guids are particularly
// awkward - it turns out that there are multiple guid representations, and I accidentally used one that
// I can only call... ""crazy-endian"". Just make sure you check the order!

            // It should not be necessary to use bcl.proto from code that uses protobuf-net

            syntax = ""proto3"";

            option csharp_namespace = ""ProtoBuf.Bcl"";

            package bcl;

            message TimeSpan {
                sint64 value = 1; // the size of the timespan (in units of the selected scale)
                TimeSpanScale scale = 2; // the scale of the timespan [default = DAYS]
  enum TimeSpanScale
        {
            DAYS = 0;
            HOURS = 1;
            MINUTES = 2;
            SECONDS = 3;
            MILLISECONDS = 4;
            TICKS = 5;

            MINMAX = 15; // dubious
        }
    }

    message DateTime
    {
        sint64 value = 1; // the offset (in units of the selected scale) from 1970/01/01
        TimeSpanScale scale = 2; // the scale of the timespan [default = DAYS]
        DateTimeKind kind = 3; // the kind of date/time being represented [default = UNSPECIFIED]
  enum TimeSpanScale
    {
        DAYS = 0;
        HOURS = 1;
        MINUTES = 2;
        SECONDS = 3;
        MILLISECONDS = 4;
        TICKS = 5;

        MINMAX = 15; // dubious
    }
    enum DateTimeKind
    {
        // The time represented is not specified as either local time or Coordinated Universal Time (UTC).
        UNSPECIFIED = 0;
        // The time represented is UTC.
        UTC = 1;
        // The time represented is local time.
        LOCAL = 2;
    }
}

message NetObjectProxy
{
    int32 existingObjectKey = 1; // for a tracked object, the key of the **first** time this object was seen
    int32 newObjectKey = 2; // for a tracked object, a **new** key, the first time this object is seen
    int32 existingTypeKey = 3; // for dynamic typing, the key of the **first** time this type was seen
    int32 newTypeKey = 4; // for dynamic typing, a **new** key, the first time this type is seen
  string typeName = 8; // for dynamic typing, the name of the type (only present along with newTypeKey)
    bytes payload = 10; // the new string/value (only present along with newObjectKey)
}

message Guid
{
    fixed64 lo = 1; // the first 8 bytes of the guid (note:crazy-endian)
    fixed64 hi = 2; // the second 8 bytes of the guid (note:crazy-endian)
}

message Decimal
{
    uint64 lo = 1; // the first 64 bits of the underlying value
    uint32 hi = 2; // the last 32 bis of the underlying value
    uint32 signScale = 3; // the number of decimal digits (bits 1-16), and the sign (bit 0)
}
";
        }
    }
}
