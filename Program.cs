using lexicana.Configurations;
using lexicana.Database;
using lexicana.Endpoints;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddConfiguration(builder);

var app = builder.Build();

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.AddAutoMigration();
}

app.UseAuthentication();
app.UseRouting();
app.UseCors(CorsConfig.CorsKey);
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapEndpoints();

app.Run();