/*using AutoMapper;
using Infrastructure.Context;
using Infrastructure.Repositories.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Movies;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.AddDbContext<NetflixRemakeContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddResponseCaching();
builder.Services.AddControllers(options =>
{
    var cacheProfiles = builder.Configuration
            .GetSection("CacheProfiles")
            .GetChildren();
    foreach (var cacheProfile in cacheProfiles)
    {
        options.CacheProfiles
        .Add(cacheProfile.Key,
        cacheProfile.Get<CacheProfile>());
    }
});

builder.Services.AddCors();
builder.Services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses (e.g. Role)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers()
            .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCaching();
app.UseRouting();

//app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseCors(x => x
        .SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
app.MapControllers();

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Movie}/{action=Index}/{id?}");
});
//app.UseAuthentication();
//app.UseAuthorization();

app.Run();
*/



using AutoMapper;
using Infrastructure.Context;
using Infrastructure.Repositories.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Movies;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1. Conectare la baza de date
var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.AddDbContext<NetflixRemakeContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Configurarea de caching
builder.Services.AddResponseCaching();

// 3. Ad?ugarea MVC
builder.Services.AddControllersWithViews(options =>
{
    var cacheProfiles = builder.Configuration
        .GetSection("CacheProfiles")
        .GetChildren();
    foreach (var cacheProfile in cacheProfiles)
    {
        options.CacheProfiles
        .Add(cacheProfile.Key,
        cacheProfile.Get<CacheProfile>());
    }
})
.AddJsonOptions(x =>
{
    // serialize enums as strings in api responses (e.g. Role)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// 4. Configurarea AutoMapper pentru mapping
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// 5. Configurarea CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors();
builder.Services.AddControllersWithViews()
            .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

// 6. Configurarea Swagger (pentru dezvoltare)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 7. Înregistrarea serviciilor pentru repository ?i service
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();

var app = builder.Build();

// 8. Configurarea pipeline-ului de request-uri
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCaching();
app.UseRouting();

// 9. Permisiuni CORS
app.UseCors(x => x
        .SetIsOriginAllowed(origin => true) // Permite toate originile
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

// 10. Configurarea rutei MVC
app.UseEndpoints(endpoints =>
{
    // Definirea rutei standard pentru MVC
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Movie}/{action=Index}/{id?}");
});

// 11. Rulare aplica?ie
app.UseStaticFiles(); // Suport pentru fi?iere statice (ex: CSS, JS, imagini)
app.MapControllers(); // Ruteaz? la controlerele API (dac? ai nevoie)
app.Run();
