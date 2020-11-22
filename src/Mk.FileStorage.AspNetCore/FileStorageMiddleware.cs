using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mk.FileStorage.AspNetCore
{
    public class FileStorageMiddleware
    {
        private readonly RequestDelegate _next;

        public FileStorageMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext httpContext, IFileStorageService fileStorageService)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (!httpContext.Request.RouteValues.TryGetValue("fileName", out var fileName))
            {
                return;
            }

            if (httpContext.Request.Method == HttpMethod.Post.Method)
            {
                await UploadAsync(httpContext, fileStorageService, (string)fileName!);
            }

            if (httpContext.Request.Method == HttpMethod.Get.Method)
            {
                await DownloadAsync(httpContext, fileStorageService, (string)fileName!);
            }
        }

        private static async Task DownloadAsync(HttpContext httpContext, IFileStorageService fileStorageService, string fileName)
        {
            try
            {
                httpContext.Response.ContentType = "application/octet-stream";
                httpContext.Response.StatusCode = StatusCodes.Status200OK;
                await fileStorageService.DownloadToAsync(fileName!, httpContext.Response.Body);
            }
            catch (FileStorageException ex) when (ex.ErrorCode == FileStorageErrorCode.FileNotFound)
            {
                httpContext.Response.ContentType = null!;
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }

        private static async Task UploadAsync(HttpContext httpContext, IFileStorageService fileStorageService, string fileName)
        {
            try
            {
                await fileStorageService.UploadAsync(fileName!, httpContext.Request.Body);
                httpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch (FileStorageException ex) when (ex.ErrorCode == FileStorageErrorCode.FileAlreadyExists)
            {
                httpContext.Response.ContentType = null!;
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
            }
        }
    }
}