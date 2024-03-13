using EventManagement.API;
using EventManagement.Application;
using EventManagement.Infrastructure.Identity;
using EventManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

bool isDevelopment = builder.Environment.IsDevelopment();


builder.Services.AddWeb(builder.Configuration);
builder.Services.AddPersistenceInfrastructure(builder.Configuration, isDevelopment);
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Migrate(); // apply any pending migration

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();