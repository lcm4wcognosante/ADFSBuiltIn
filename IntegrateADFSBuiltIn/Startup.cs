using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IntegrateADFSBuiltIn
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthentication(configureOptions: x =>
            {
                //x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
            })
                .AddWsFederation(options =>
                {
                    options.Wtrealm = "api://2b840c74-674a-424b-905e-04f37f78bba9";
                    options.MetadataAddress = "https://sts-dev.365.cognosante.cc/FederationMetadata/2007-06/FederationMetadata.xml";
                    //options.CallbackPath = new PathString("/.auth/login/custom");
                })
                .AddCookie();
            //.AddJwtBearer(options =>
            //{
            //    options.SaveToken = true;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:Key"])),
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        RequireExpirationTime = false,
            //        ValidateLifetime = true
            //    };
            //})
            //.AddWsFederation("wsfed", options =>
            //{
            //    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //    options.SignOutScheme = IdentityServerConstants.SignoutScheme;

            //    options.MetadataAddress = "https://sts-dev.365.cognosante.cc/FederationMetadata/2007-06/FederationMetadata.xml";
            //    options.Wtrealm = "api://2b840c74-674a-424b-905e-04f37f78bba9";

            //    options.CallbackPath = new PathString("/.auth/login/custom");
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            //app.UseStatusCodePages(async context =>
            //{
            //    var response = context.HttpContext.Response;

            //    if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
            //    {
            //        response.Redirect("Login/Login");
            //    }
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
