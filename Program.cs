var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.UseMiddleware<LogMiddleware>();

app.MapGet("/", () => "Map Get!");
app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("middleware (before map check) req in\n");
    var condition = true;
    if (condition)
    {
        await next();
    }
    await context.Response.WriteAsync("middleware (before map check) req out\n");
});

app.Map("/map1", appBuilder =>
{
    appBuilder.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("middleware map1 (req in)\n");
        await next.Invoke();
        await context.Response.WriteAsync("middleware map1 (req out)\n");
    });

    appBuilder.Run(async context =>
    {
        await context.Response.WriteAsync("middleware map1\n");
    });
});

app.Map("/map2", appBuilder =>
{
    appBuilder.Run(async context =>
    {
        await context.Response.WriteAsync("middleware map2\n");
    });
});

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("middleware (after map check) req in\n");
    var condition = true;
    if (condition)
    {
        await next();
    }
    await context.Response.WriteAsync("middleware (after map check) req out\n");
});

app.Run(async context =>
{
    await context.Response.WriteAsync("middleware end\n");
});

app.Run();
