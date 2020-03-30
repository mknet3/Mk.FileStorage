using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Mk.FileStorage
{
    public static class FileStorageEndpointRouteBuilderExtensions
    {
        private const string DefaultDisplayName = "File Storage";
        private const string DefaultPattern = "files";

        public static IEndpointConventionBuilder MapMkFileStorage(this IEndpointRouteBuilder endpoints)
        {
            return MapMkFileStorage(endpoints, DefaultPattern);
        }

        public static IEndpointConventionBuilder MapMkFileStorage(this IEndpointRouteBuilder endpoints, string pattern)
        {
            if (endpoints == null) throw new ArgumentNullException(nameof(endpoints));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            var pipeline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<FileStorageMiddleware>()
                .Build();

            pattern = pattern.TrimEnd('/') + "/{fileName}";
            return endpoints.Map(pattern, pipeline).WithDisplayName(DefaultDisplayName);
        }
    }
}