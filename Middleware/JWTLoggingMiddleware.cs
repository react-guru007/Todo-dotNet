public class JwtLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public JwtLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Check if the request has an Authorization header
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var token = context.Request.Headers["Authorization"].ToString();
            Console.WriteLine("Incoming JWT Token: " + token);
        }

        // Call the next middleware in the pipeline
        await _next(context);
    }
}