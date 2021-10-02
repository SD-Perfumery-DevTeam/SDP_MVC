using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore.Models.DbContext;
using Microsoft.SDP.SDPInfrastructure.Services;
using System;

namespace Global
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
            services.AddMvc();
            services.AddSession(o => {
                o.IdleTimeout = TimeSpan.FromMinutes(60);
               
            });
            services.AddMemoryCache();
            services.AddDbContext<ProductDbContext>(options =>
            {
                string conString = Configuration.GetValue<string>("ConnectionStrings:PostConnCtion");
                options.UseSqlServer(conString);
            });
            services.AddIdentity<IdentityUser, IdentityRole>()
              .AddEntityFrameworkStores<ProductDbContext>()
              .AddDefaultTokenProviders();
            
            services.AddScoped<IDbRepo, DbRepo>();//database repo
            services.AddScoped<IEmailSender, EmailService>();//email sender service
            services.AddScoped<ICustomer, GuestCustomerService>(); //guest customers service
            services.AddScoped<ImageService, ImageService>();//image storage service

            services.Configure<IdentityOptions>(options =>
            {
                // Default User settings.
                options.User.AllowedUserNameCharacters =
                        "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ @._-";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
               /* app.UseExceptionHandler("/Home/Error");*/
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
