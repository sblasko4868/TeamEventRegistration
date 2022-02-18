using TeamEventRegistration.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using TeamEventRegistration.Core.Services;
using TeamEventRegistration.Core.Enumerations;
using TeamEventRegistration.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using TeamEventRegistration.Core.Authorization;
using TeamEventRegistration.Core.Infrastructure;
using Ganss.XSS;

namespace TeamEventRegistration.Web
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
            services.AddDbContext<TeamEventRegistrationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<Member, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<TeamEventRegistrationDbContext>()
                .AddRoles<IdentityRole>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationEnumerations.Policies.RequireSystemAdministrator.ToString(), policy =>
                {
                    policy.RequireRole(AuthorizationEnumerations.Roles.SystemAdministrator.ToString());
                });
                options.AddPolicy(AuthorizationEnumerations.Policies.RequireEventAdministrator.ToString(), policy =>
                {
                    policy.RequireRole(
                        AuthorizationEnumerations.Roles.SystemAdministrator.ToString(),
                        AuthorizationEnumerations.Roles.EventAdministrator.ToString());
                });
                options.AddPolicy(AuthorizationEnumerations.Policies.RequireAuthorized.ToString(), policy =>
                {
                    policy.RequireAuthenticatedUser();
                });
            });
            services.AddTransient<IEmailSender>(s => new EmailService(
                new Uri(new Uri(Configuration["AppSettings:EmailApi:BaseUri"]), Configuration["AppSettings:EmailApi:Endpoint"]),
                Configuration["AppSettings:EmailApi:Key"])
            );
            services.AddScoped<DbContext, TeamEventRegistrationDbContext>();

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeAreaFolder("Registration", "/", AuthorizationEnumerations.Policies.RequireAuthorized.ToString());
                options.Conventions.AuthorizeAreaFolder("Event", "/", AuthorizationEnumerations.Policies.RequireAuthorized.ToString());
                options.Conventions.AuthorizeAreaFolder("Admin", "/", AuthorizationEnumerations.Policies.RequireEventAdministrator.ToString());
                options.Conventions.AuthorizeAreaFolder("Admin", "/Members", AuthorizationEnumerations.Policies.RequireSystemAdministrator.ToString());
                options.Conventions.AuthorizeAreaFolder("Admin", "/Roles", AuthorizationEnumerations.Policies.RequireSystemAdministrator.ToString());
                options.Conventions.AllowAnonymousToFolder("/");

                options.Conventions.AddAreaPageRoute("Event", "/Activities", "/Event/{id}/Activities");
            });
            services.AddControllers();
            services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("AppSettings:GoogleOAuth");

                    googleOptions.ClientId = googleAuthNSection["ClientId"];
                    googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                })
                .AddMicrosoftAccount(microsoftOptions =>
                {
                    IConfigurationSection microsoftAuthSection =
                        Configuration.GetSection("AppSettings:GoogleOAuth");

                    microsoftOptions.ClientId = microsoftAuthSection["ClientId"];
                    microsoftOptions.ClientSecret = microsoftAuthSection["ClientSecret"];
                });
            services.AddTransient<ZohoRegistrationService>();
            services.AddScoped<IAuthorizationHandler, TeamOperationHandler>();
            services.AddScoped<IAuthorizationHandler, EventOperationHandler>();
            services.AddTransient<Func<RegistrationRequirementEnumerations.RegistrationRequirementNames, IRegistrationService>>( serviceProvider => key =>
            {
                switch (key)
                {
                    case RegistrationRequirementEnumerations.RegistrationRequirementNames.BeerOlympicsRegistrationSignature:
                        return serviceProvider.GetService<ZohoRegistrationService>();
                    default:
                        throw new KeyNotFoundException();
                }
            });
            services.AddTransient<DbInitializer>();
            services.AddSingleton<IHtmlSanitizer, HtmlSanitizer>(_ => new HtmlSanitizer());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Development-Local"))
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages().RequireAuthorization();
                endpoints.MapControllers();
            });
        }
    }
}
