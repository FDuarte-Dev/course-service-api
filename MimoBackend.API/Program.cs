using MimoBackend.API;
using MimoBackend.API.Middlewares;
using MimoBackend.API.Persistence;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Custom DI
builder.Services.SetRepositories();
builder.Services.SetPersistence();
builder.Services.SetServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseWhen(RequireAuthorization, appBuilder =>
{
    appBuilder.UseMiddleware<UserAuthorizationMiddleware>();
});

SetSerializationRules();
PopulateDb(app);

app.Run();
return;

bool RequireAuthorization(HttpContext httpContext) 
    => httpContext.Request.Path.StartsWithSegments("/lessons");
    
void SetSerializationRules()
{
    JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
    {
        Formatting = Formatting.Indented,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };
}

static void PopulateDb(IApplicationBuilder app)
{
    using (var scope = app.ApplicationServices.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.PopulateDb();
    }
}