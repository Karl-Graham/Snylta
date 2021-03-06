﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snylta.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snylta.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Snylta.Services;
using System.IO;

namespace Snylta
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment host)
        {
            Configuration = configuration;
            _host = host;
        }

        public IConfiguration Configuration { get; }
        private IHostingEnvironment _host;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            AppSettings appConfiguration = Configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddSingleton(appConfiguration);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"));

                options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            //Begär authenticaction som default. För att kringgå addare [AllowAnonymous] (Elin)
            services.AddMvc(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            });
            //services.AddMvc();

            services.AddTransient<TranslationService>();
            services.AddTransient<ImageTagGeneratorService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            if (!Directory.Exists(_host.WebRootPath + "\\thingimages\\"))
                Directory.CreateDirectory(_host.WebRootPath + "\\thingimages\\");
            if (!Directory.Exists(_host.WebRootPath + "\\CameraPhotos\\"))
                Directory.CreateDirectory(_host.WebRootPath + "\\CameraPhotos\\");
            if (!Directory.Exists(_host.WebRootPath + "\\groupimages\\"))
                Directory.CreateDirectory(_host.WebRootPath + "\\groupimages\\");

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                //app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();

                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
