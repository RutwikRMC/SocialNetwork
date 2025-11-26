using SocialNetwork.Repository;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Read Render dynamic port
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173") // React app origin
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// Add services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IRegistrationManager, RegistrationManager>();
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<IArticleManager, ArticleManager>();

var app = builder.Build();

// HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();

// ✅ Add root URL for testing
app.MapGet("/", () => "SocialNetwork API is running!");

// Run the app
app.Run();
