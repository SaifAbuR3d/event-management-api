using EventManagement.Infrastructure.Identity;
using EventManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

bool isDevelopment = builder.Environment.IsDevelopment();


builder.Services.AddControllers();


builder.Services.AddPersistenceInfrastructure(builder.Configuration, isDevelopment);
builder.Services.AddIdentityInfrastructure(builder.Configuration);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();