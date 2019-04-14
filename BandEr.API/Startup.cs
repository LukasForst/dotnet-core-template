using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BandEr.API.AuthRequirements;
using BandEr.API.Middlewares;
using BandEr.BL.Facades;
using BandEr.Common.Options;
using BandEr.DAL;
using BandEr.DAL.Entity;
using BandEr.DAL.Extensions;
using BandEr.Infrastructure.Auth.Interfaces;
using BandEr.Infrastructure.Auth.Services;
using BandEr.Infrastructure.Mapping;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace BandEr.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        protected virtual void SetupDbContext(IServiceCollection services)
        {
            services.AddDbContext<BandErDbContext>(builder =>
                builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        protected virtual void EmailServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(c => { c.CreateMissingTypeMaps = true; }, typeof(ValueProfile));
            AuthServices(services);
            ConfigureSwagger(services, true);
            SetupDbContext(services);
            EmailServices(services);            
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<TodoFacade>());

            services.AddScoped<TodoFacade>();
        }

        private void ConfigureSwagger(IServiceCollection services, bool generateDocs = false)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "BandEr API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    In = "header",
                    Description = "Please enter your JWT token into field e.g. Bearer abcdefgh",
                    Name = "Authorization",
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    {
                        "Bearer", Enumerable.Empty<string>()
                    },
                });

                if (generateDocs)
                {
                    // Set the comments path for the Swagger JSON and UI.
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                }
            });
        }

        private void ConfigureSwagger(IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Host = httpReq.Host.Value;
                });
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BandEr API V1");
                c.RoutePrefix = string.Empty;
            });
        }

        private void AuthServices(IServiceCollection services)
        {
            services.AddScoped<IJwtFactory, JwtFactory>();
            services.AddScoped<ITokenFactory, TokenFactory>();
            services.AddScoped<IJwtTokenValidator, JwtTokenValidator>();
            services.AddSingleton(typeof(IRefreshTokenService<,>), typeof(InMemoryRefreshTokenService<,>));
            services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();

            var authSettings = Configuration.GetSection(nameof(AuthSettings));
            services.Configure<AuthSettings>(authSettings);
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings[nameof(AuthSettings.SecretKey)]));
            // Configure JwtIssuerOptions            
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                options.ValidFor = TimeSpan.Parse(jwtAppSettingOptions[nameof(JwtIssuerOptions.ValidFor)]);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;

                configureOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.Requirements.Add(new ValidApiUseRequirement()));
            });

            // add identity
            var identityBuilder = services.AddIdentityCore<AppUser>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireNonAlphanumeric = true;
                o.Password.RequiredLength = 6;
                o.User.RequireUniqueEmail = true;
            });

            identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(AppRole), identityBuilder.Services);
            identityBuilder.AddEntityFrameworkStores<BandErDbContext>().AddDefaultTokenProviders();

            //add provider
            services.AddScoped<ITenantProvider, TokenTenantProvider>();
        }

        protected virtual void ConfigureErrorHandling(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
#if DEBUG
                app.UseDeveloperExceptionPage();
#else
                app.UseErrorHandlingMiddleware();                
#endif
            }
            else
            {
                app.UseErrorHandlingMiddleware();
            }
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="mapper"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper mapper)
        {
            ConfigureSwagger(app);
            InitializeDatabase(app);
            ConfigureErrorHandling(app, env);

            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            app.UseHsts();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        protected virtual void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var log = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
                var context = scope.ServiceProvider.GetRequiredService<BandErDbContext>();
                var um = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                context.Database.Migrate();
                context.SeedAsync(um).Wait();
            }
        }
    }
}
