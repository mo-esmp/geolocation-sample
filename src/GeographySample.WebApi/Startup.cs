using GeographySample.Core;
using GeographySample.Core.User;
using GeographySample.Infrastructure.Data;
using GeographySample.Infrastructure.User;
using GeographySample.WebApi.Authentication.Jwt;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NServiceBus;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace GeographySample.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<GeographyDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                    npgsqlOptions => npgsqlOptions.UseNetTopologySuite().MigrationsAssembly("GeographySample.Infrastructure")));

            services.AddApiVersioning(options => { options.ReportApiVersions = true; });

            services.AddVersionedApiExplorer(options =>
               {
                   options.GroupNameFormat = "'v'VVV";
                   options.SubstituteApiVersionInUrl = true;
               });

            services.AddMediatR(typeof(UserCreateCommand), typeof(UserCommandHandler));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            AddAuthentication(services);

            AddSwagger(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostApplicationLifetime applicationLifetime)
        {
            app.UseApiExceptionHandling();

            app.UseHttpsRedirection();

            app.UseSwagger(options => { options.RouteTemplate = "docs/{documentName}/docs.json"; })
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/docs/v1/docs.json", "Geography Sample API v1");
                    options.RoutePrefix = "docs";
                });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            applicationLifetime.ApplicationStarted.Register(() =>
            {
                var endpointInstance = app.ApplicationServices.GetService<IMessageSession>();
            });
        }

        private void AddAuthentication(IServiceCollection services)
        {
            services.AddIdentity<UserEntity, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<GeographyDbContext>().AddDefaultTokenProviders();

            services.AddScoped<JwtTokenGenerator>();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            ValidAudience = Configuration["Jwt:Audience"],
                            IssuerSigningKey = JwtKeyGenerator.Generate(Configuration["Jwt:SecretKey"])
                        };
                });
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}