using AutoMapper;
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
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
//app.UseAuthentication();
//app.UseAuthorization();

app.Run();
