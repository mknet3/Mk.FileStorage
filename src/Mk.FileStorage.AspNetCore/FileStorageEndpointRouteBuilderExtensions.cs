using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Mk.FileStorage.AspNetCore
{
    public static class FileStorageEndpointRouteBuilderExtensions
    {
        private const string DefaultDisplayName = "File Storage";
        private const string DefaultPattern = "files";

        public static IEndpointConventionBuilder MapMkFileStorage(this IEndpointRouteBuilder endpoints)
        {
            return MapMkFileStorage(endpoints, DefaultPattern);
        }

        public static IEndpointConventionBuilder MapMkFileStorage(this IEndpointRouteBuilder endpoints, string filesPath)
        {
            if (endpoints == null) throw new ArgumentNullException(nameof(endpoints));
            if (filesPath == null) throw new ArgumentNullException(nameof(filesPath));

            var pipeline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<FileStorageMiddleware>()
                .Build();

            var pattern = filesPath.TrimEnd('/') + "/{fileName}";
            return endpoints.Map(pattern, pipeline).WithDisplayName(DefaultDisplayName);
        }
    }
}