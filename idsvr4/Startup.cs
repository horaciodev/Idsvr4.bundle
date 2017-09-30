using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Genesis.idlib.Data;
using Genesis.idlib.Models;

using idsvr4.Services;
using idsvr4.Infrastructure;

using IdentityServer4;
using IdentityServer4.Services;
using System.Security.Cryptography.X509Certificates;

namespace idsvr4
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            builder.AddEnvironmentVariables();    
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

             //add application services
            services.AddTransient<IProfileService, IdentityClaimsProfileService>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

             //add Identity Server4
            services.AddIdentityServer()
                .AddSigningCredential(LoadCertFromStore())
                .AddConfigurationStore(builder => 
                    builder.UseSqlServer(sqlConnStr))
                .AddOperationalStore(builder =>
                    builder.UseSqlServer(sqlConnStr))                  
                .AddAspNetIdentity<ApplicationUser>();
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
             var sqlConnStr = Configuration.GetConnectionString("IdentityServer4DB");
            // Add framework services.
            services.Configure<MvcOptions>(options =>{
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(sqlConnStr));

            services.AddIdentity<ApplicationUser,ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext,long>()
                .AddDefaultTokenProviders();

            services.AddMvc();

             //add application services
            services.AddTransient<IProfileService, IdentityClaimsProfileService>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

             //add Identity Server4
            services.AddIdentityServer()
                .AddSigningCredential(LoadCertFromStore(true))
                .AddConfigurationStore(builder => 
                    builder.UseSqlServer(sqlConnStr))
                .AddOperationalStore(builder =>
                    builder.UseSqlServer(sqlConnStr))                  
                .AddAspNetIdentity<ApplicationUser>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                var options = new RewriteOptions().AddRedirectToHttps();
                app.UseRewriter(options);
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private X509Certificate2 LoadCertFromStore(bool isDevelopment=false)
        {
            X509Certificate2 x509Cert = null;
            X509Store certStore = null; 

            try
            {
                if(!isDevelopment) //Note Development environment is cloud development environment
                    certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                else
                    certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);

                var idSvr4ConfigSettings = Configuration.GetSection("IdSrv4Settings");
                var certThumbPrint = idSvr4ConfigSettings.GetValue<string>("TokenSigningCertificateThumbPrint");

                certStore.Open(OpenFlags.ReadOnly);

                var certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, certThumbPrint, false);

                if (0 == certCollection.Count)
                {
                    throw new Exception("No certificate was found containing specified thumbprint");
                }

                if(!isDevelopment)
                {
                string certPwd = idSvr4ConfigSettings.GetValue<string>("signing-certificate.password");

                byte[] certBytes = certCollection[0].Export(X509ContentType.Pkcs12, certPwd);

                x509Cert = new X509Certificate2(certBytes, certPwd
                                                , X509KeyStorageFlags.MachineKeySet);                
                }
                else
                {
                    x509Cert = certCollection[0];
                }
            }            
            finally
            {
                
                certStore.Dispose();
            }

            return x509Cert;
        }
    }
}
