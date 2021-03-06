﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Test.Interface;
using Test.Middleware;
using Test.Models;
using Test.Extension;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Test.XSS;

namespace Test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var appConfig = new SetModel();
            configuration.GetSection("SetModel").Bind(appConfig);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddModuelConfig(Configuration);

            services.AddRedis(options => {
                options.InstanceName = Configuration["Scheme:Default"];
                options.ConnectionString = Configuration["Redis:Default"];
            });

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = Configuration["Scheme:Default"];
                options.DefaultChallengeScheme = Configuration["Scheme:Default"];
            }).AddScheme<AuthenticationSchemeOptions, OAuthHandler>(Configuration["Scheme:Default"], null);

            services.AddMvc(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                            .AddAuthenticationSchemes(Configuration["Scheme:Default"])
                            .RequireAuthenticatedUser()
                            .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                }
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<SetModel>(Configuration.GetSection("SetModel"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, @"wwwroot")),
                    RequestPath = new PathString("/files"),
                    EnableDirectoryBrowsing = true
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMiddleware<TestMiddleware>();
            app.UseAuthentication();
            app.AddXSS(options =>
            {
                options.Scripts.AllowAny();
                options.Styles.AllowAny();
                options.Frames.Disable();
                options.Media.AllowSelf();
                options.Images.AllowSelf();
                options.FrameAncestors.Disable();
                options.Connects.AllowSelf();
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}