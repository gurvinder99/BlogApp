namespace BlogApp.Middleware;

public class RequestTimeMiddleware
{
    private readonly RequestDelegate _next;

    public RequestTimeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;

        await _next(context);

        var endTime = DateTime.UtcNow;
        var duration = endTime - startTime;
        Console.WriteLine($"Request [{context.Request.Method} {context.Request.Path}] took {duration.TotalMilliseconds} ms");
    }
}
