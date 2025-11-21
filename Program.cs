using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<AlphaClient>();
builder.Services.AddControllers();

// Set URL to http://localhost:5000
builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();

// app.UseHttpsRedirection(); // Disabled since it's a local test app
app.MapControllers();
app.Run();