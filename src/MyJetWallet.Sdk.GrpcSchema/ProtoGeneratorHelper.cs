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
            using var reader = new StreamReader("bcl.proto");
            var bcl = reader.ReadToEnd();

            endpoints.Map("/grpc",
                async context =>
                {
                    await context.Response.WriteAsync(GetListOfServices());
                });

            endpoints.Map("/grpc/protobuf-net/bcl.proto", async context =>
            {
                await context.Response.WriteAsync(bcl);
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
    }
}
