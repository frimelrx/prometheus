using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<AlphaClient>();
builder.Services.AddControllers();

var app = builder.Build();

// app.UseHttpsRedirection(); // Disabled since it's a local test app
app.MapControllers();
app.Run();