using eWallet;
using eWallet.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

public static class ServiceExtension
{    
    public static IServiceCollection AddMyAuthentication(this IServiceCollection Services, IConfiguration configuration)
    {
        Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
         .AddJwtBearer(options =>
         {
             options.SaveToken = true;
             options.RequireHttpsMetadata = false;
             options.TokenValidationParameters = new TokenValidationParameters()
             {
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidAudience = configuration["AccessConfiguration:Audience"],
                 ValidIssuer = configuration["AccessConfiguration:Issuer"],
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Vars.TheSecretKey))
             };
         });
        return Services;
    }

    

    public static IServiceCollection AddMyIdentity(this IServiceCollection Services) 
    {
        Services.AddIdentity<ApplicationUser, ApplicationRole>(_ =>
        {
            _.Password.RequiredLength = 6;
            _.Password.RequireNonAlphanumeric = false;
            _.Password.RequireLowercase = false;
            _.Password.RequireUppercase = false;
            _.Password.RequireDigit = false;
        })
         .AddRoles<ApplicationRole>()
         .AddRoleManager<RoleManager<ApplicationRole>>()
         .AddUserManager<UserManager<ApplicationUser>>()
         .AddEntityFrameworkStores<WalletDbContext>()
         .AddDefaultTokenProviders()
         .AddClaimsPrincipalFactory<MyUserClaimsPrincipalFactory>();

        return Services;
    }

    public static IServiceCollection AddMySwaggerGen(this IServiceCollection Services)
    {
        Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Wallet API",
                Description = "A simple example ASP.NET Core Web API 7.0",
                TermsOfService = new Uri("https://solid.uz/terms"),
                Contact = new OpenApiContact { Name = "Mamatqulov Sarvar", Email = string.Empty, Url = new Uri("https://twitter.com/@orppsarva") },
                License = new OpenApiLicense { Name = "Use under SOLIDCode", Url = new Uri("https://solid-code.uz/license") }
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },new string[] {}
                    }

                    });
        });
        return Services;
    }
}
