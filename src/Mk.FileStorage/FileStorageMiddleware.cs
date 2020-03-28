using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Mk.FileStorage
{
    public class FileStorageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IFileStorageService _fileStorageService;

        public FileStorageMiddleware(
            RequestDelegate next,
            IFileStorageService fileStorageService)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(fileStorageService));
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (!httpContext.Request.RouteValues.TryGetValue("fileName", out object fileName))
            {
                return;
            }

            if (httpContext.Request.Method == HttpMethod.Post.Method)
            {
                await _fileStorageService.UploadAsync((string)fileName, httpContext.Request.Body);
                httpContext.Response.StatusCode = StatusCodes.Status204NoContent;
            }

            if (httpContext.Request.Method == HttpMethod.Get.Method)
            {
                try
                {
                    httpContext.Response.ContentType = "application/octet-stream";
                    await _fileStorageService.DownloadToAsync((string)fileName, httpContext.Response.Body);
                }
                catch (FileStorageException ex)
                {
                    if (ex.ErrorCode == FileStorageErrorCode.FileNotFound)
                    {
                        httpContext.Response.ContentType = null;
                        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}