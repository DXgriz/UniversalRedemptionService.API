using System.Text.Json;

namespace UniversalRedemptionService.API.Middleware
{
    public class ResponseEnvelope(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            // Skip non-API responses
            if (!context.Request.Path.StartsWithSegments("/api"))
            {
                await next(context);
                return;
            }

            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var bodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            // Do not wrap error responses (handled by exception middleware)
            if (context.Response.StatusCode >= 400 || string.IsNullOrWhiteSpace(bodyText))
            {
                await responseBody.CopyToAsync(originalBodyStream);
                return;
            }

            object? data;
            try
            {
                data = JsonSerializer.Deserialize<object>(bodyText);
            }
            catch
            {
                data = bodyText;
            }

            var wrappedResponse = new
            {
                success = true,
                data
            };

            context.Response.ContentType = "application/json";
            context.Response.Body = originalBodyStream;

            await context.Response.WriteAsync(JsonSerializer.Serialize(wrappedResponse));
        }
    }
}
