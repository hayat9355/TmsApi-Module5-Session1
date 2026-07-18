using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using TmsApi.Data;
using TmsApi.Entities;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//  Session 1: Authentication 
builder.Services
    .AddAuthentication("Training")
    .AddScheme<AuthenticationSchemeOptions, TrainingAuthHandler>("Training", null);

builder.Services.AddDbContext<TmsDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging());

builder.Services.AddAuthorization();

//  Session 2: DI lifetime validation 
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

//  Session 2: Service registrations 
builder.Services.AddScoped<EnrollmentWorker>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();


var app = builder.Build();


//  Session 1: Logging middleware — must be outermost 
app.UseMiddleware<RequestLoggingMiddleware>();

//  Session 3: Environment toggle 
// if (app.Environment.IsDevelopment())
// {
    app.MapOpenApi();
    app.MapScalarApiReference();
// }
// else
// {
//     app.UseExceptionHandler();
// }

app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//  Session 3: Wire all controllers 
app.MapControllers();




app.Run();
