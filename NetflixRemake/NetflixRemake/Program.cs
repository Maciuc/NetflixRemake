using AutoMapper;
using Infrastructure.Context;
using Infrastructure.Repositories.Movies;
using Infrastructure.Repositories.ViewsValueRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Movies;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.AddDbContext<NetflixRemakeContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddResponseCaching();

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
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors();
builder.Services.AddControllersWithViews()
            .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();

builder.Services.AddScoped<IViewsValueRepository, ViewsValueRepository>();


var app = builder.Build();

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

app.UseCors(x => x
        .SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

app.UseStaticFiles();
app.MapControllers();
app.Run();
