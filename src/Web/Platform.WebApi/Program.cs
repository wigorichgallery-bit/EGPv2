// ===========================================
// File Location :
// src/Web/Platform.WebApi/Program.cs
// ===========================================
using Platform.WebApi.Composition;
using Platform.WebApi.DependencyInjection;
using Platform.WebApi.Middleware;

var builder =
    WebApplication.CreateBuilder(
        args);

builder.Services.AddWebApi(
    builder.Configuration);

var app =
    builder.Build();

await IdentityRoleSeeder.SeedAsync(
    app.Services);  

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseEnterpriseMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();