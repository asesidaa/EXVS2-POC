using System.Net;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Server.Middlewares;

public class AllNetRequestMiddleware
{
    private readonly RequestDelegate next;

    public AllNetRequestMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method != WebRequestMethods.Http.Post)
        {
            context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
            return;
        }

        if (!context.Request.HasFormContentType)
        {
            await next(context);
            return;
        }
        
        context.Request.EnableBuffering();
        var bodyAsText = await new StreamReader(context.Request.Body).ReadToEndAsync();
        var compressed = Convert.FromBase64String(bodyAsText);
        var decompressed = Decompress(compressed);
        context.Request.Body.Position = 0;
        context.Request.Body = decompressed;
        context.Request.ContentLength = decompressed.Length;

        // Call the next delegate/middleware in the pipeline.
        await next(context);
    }
    
    private static Stream Decompress(byte[] data)
    {
        var outputStream = new MemoryStream();
        using var compressedStream = new MemoryStream(data);
        using var inputStream = new InflaterInputStream(compressedStream);
        inputStream.CopyTo(outputStream);
        outputStream.Position = 0;
        return outputStream;
    }
}

public static class AllNetMiddlewareExtensions
{
    public static IApplicationBuilder UseAllNetRequestMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AllNetRequestMiddleware>();
    }
}