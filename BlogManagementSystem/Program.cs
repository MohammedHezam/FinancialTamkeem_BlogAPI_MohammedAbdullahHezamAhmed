using BlogManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using BlogManagementSystem.Services;
using System.Text;
using BlogManagementSystem.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<dbContext>(c =>
{
    c.UseSqlServer(builder.Configuration.GetConnectionString("dbConnection"));
    c.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    //c.EnableSensitiveDataLogging(false);

}, ServiceLifetime.Singleton);

//builder.Services.AddDbContext<dbContext>(c => c.UseSqlServer(builder.Configuration.GetConnectionString("dbConnection")), ServiceLifetime.Singleton);

//var appSettingsData = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

//if (appSettingsData != null)
//{
//    if (!string.IsNullOrEmpty(appSettingsData.TokenSecretKey))
//    {
//        var keys = Encoding.ASCII.GetBytes(appSettingsData.TokenSecretKey);

//        if (keys != null)
//        {
//            builder.Services.AddAuthentication(x =>
//            {

//                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//            }).AddJwtBearer(x =>
//            {
//                x.RequireHttpsMetadata = false;
//                x.SaveToken = true;
//                x.TokenValidationParameters = new TokenValidationParameters
//                {
//                    RequireExpirationTime = true,
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(keys),
//                    ValidateIssuer = false,
//                    ValidateAudience = false

//                };
//            });
//        }
//    }



//}

builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        //x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    })
    .ConfigureApiBehaviorOptions(opt =>
    {
        opt.InvalidModelStateResponseFactory = context =>
        {
            var responseObj = new
            {
                //title = SharingCost.Resources.Messages.somethingErrorOccurred,
                path = context.HttpContext.Request.Path.ToString(),
                method = context.HttpContext.Request.Method,
                controller = (context.ActionDescriptor as ControllerActionDescriptor)?.ControllerName,
                action = (context.ActionDescriptor as ControllerActionDescriptor)?.ActionName,
                errors = context.ModelState.Keys.Select(k =>
                {
                    return new
                    {
                        field = k,
                        Messages = context.ModelState[k]?.Errors.Select(e => e.ErrorMessage)
                    };
                })
            };

            return new BadRequestObjectResult(responseObj);
        };
    })
    // to disable the automatic responses altogether (InvalidModelStateResponseFactory) Up method (true=> disable, false=> active
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = false;
    });

#region AddScoped 

builder.Services.AddTransient<IBlogPostService, BlogPostService>();

#endregion

var app = builder.Build();


var supportedCultures = new[] { "en", "ar" };
var localizationOptions = new RequestLocalizationOptions();
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

localizationOptions.AddSupportedCultures(supportedCultures);
localizationOptions.AddSupportedUICultures(supportedCultures);

localizationOptions.SupportedCultures[1].NumberFormat = localizationOptions.SupportedCultures[0].NumberFormat;
localizationOptions.SupportedUICultures[1].NumberFormat = localizationOptions.SupportedUICultures[0].NumberFormat;

localizationOptions.SetDefaultCulture(supportedCultures[1]);

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.MapControllers();

app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseMiddleware<JwtMiddleware>();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<dbContext>();
    db.Database.Migrate();

    //sharingCostDb.Database.MigrateAsync();
    //db.Update(db);

}
app.Run();
