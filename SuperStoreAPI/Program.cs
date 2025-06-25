using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SuperStoreAPI.EndPoints;
using SuperStoreAPI.Services.ProductService;
using SuperStoreAPI.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.Audience = builder.Configuration.GetValue<string>("Keycloak:Audience");
        options.MetadataAddress = builder.Configuration.GetValue<string>("Keycloak:MetadataAddress")!;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration.GetValue<string>("Keycloak:ValidIssuer"),
        };
    });

builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapGet("/users/me", (ClaimsPrincipal cp) =>
{
    return Results.Ok(cp.Claims.ToDictionary(x => x.Type, x => x.Value));
});

app.MapUserEndpoints();
app.MapEndPoints();

app.Run();

