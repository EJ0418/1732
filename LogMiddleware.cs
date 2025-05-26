public class LogMiddleware
{
    private readonly RequestDelegate _next;

    public LogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"Req: {context.Request.Path}");

        if (context.Request.Path == "/hello")
        {
            await context.Response.WriteAsync("HIHIHI!");
        }
        else
        {
            await _next(context);
        }
    }

    
}