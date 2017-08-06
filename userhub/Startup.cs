using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;



using Genesis.idlib.Data;
using Genesis.idlib.Models;
using Genesis.idlib.Repositories;
using Genesis.idlib.Services;

using userhub.Infrastructure;
using userhub.Infrastructure.Services;

using IdentityModel;


namespace userhub
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var sqlConnStr = Configuration.GetConnectionString("IdentityServer4DB");
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(sqlConnStr));

            services.AddIdentity<ApplicationUser,ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext,long>()
                .AddDefaultTokenProviders(); 

            services.AddMvc();

            services.AddOptions();

            services.AddAuthorization(options => {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("role","Poseidon"));
            });

            //add application services
            services.AddTransient<IModelDataService,ModelDataService>();
            services.AddTransient<IUserDataService, UserDataService>();
            services.AddTransient<IUserRepository, UserRepository>(svcProvider =>{
                return new UserRepository(sqlConnStr);
            });
            services.Configure<ConfigAppSettings>(Configuration.GetSection("ConfigAppSettings"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            #region beforeMVC
            //the following two should always go before MVC in the pipeline
            app.UseCookieAuthentication(new CookieAuthenticationOptions{
                AuthenticationScheme = "Cookies", //Cookies or cookie?
                AutomaticAuthenticate = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(60)
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions{
                AuthenticationScheme = "oidc",
                SignInScheme = "Cookies",
                Authority = "http://localhost:5000",
                RequireHttpsMetadata = false,
                ClientId = "userhub",
                //ClientSecret = "secret",
                ResponseType = "id_token", //"code id_token"
                Scope = {"openid","profile","email","role"},
                GetClaimsFromUserInfoEndpoint = true,
                SaveTokens = true,

                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role
                },
            });

            #endregion

            app.UseMvcWithDefaultRoute();
        }
    }
}
