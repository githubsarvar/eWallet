using eWallet;
using eWallet.Infrastructure;
using eWallet.Services;
using FlakeyBit.DigestAuthentication.AspNetCore;
using FlakeyBit.DigestAuthentication.Implementation;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
string CorsPolicyOrigins = "_CorsPolicyOrigins";
builder.Services.AddMediatR(typeof(Program));

builder.Services.AddScoped<IUsernameHashedSecretProvider, UsernameSecretProvider>();
builder.Services.AddAuthentication("Digest")
        .AddDigestAuthentication(DigestAuthenticationConfiguration.Create("VerySecret", "some-realm", 60, true, 20));

builder.Services.AddMyIdentity();
builder.Services.AddMyAuthentication(configuration);
builder.Services.AddMySwaggerGen();
builder.Services.AddDbContext<WalletDbContext>(option => option.UseNpgsql(configuration["ConnectionStrings:NpgsqlConnectionString"]));
builder.Services.AddScoped<IUserProvider, UserProvider>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
await app.UseRoleInitializer();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
