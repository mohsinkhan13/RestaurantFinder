using System;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mohsin.JustEat.Services;

namespace Mohsin.JustEat.WebApp
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
            services.AddControllersWithViews();
            services.Configure<BaseUrlsOptions>(Configuration.GetSection("BaseUrls"));
            services.Configure<RelativeUrlsOptions>(Configuration.GetSection("RelativeUrls"));
            services.Configure<CredentialsOptions>(Configuration.GetSection("Credentials"));

            services.AddHttpClient<IRestaurantFinder, RestaurantFinder>(client =>
            {
                var baseUrls = new BaseUrlsOptions();
                var creds = new CredentialsOptions();

                Configuration.GetSection("BaseUrls").Bind(baseUrls);
                Configuration.GetSection("Credentials").Bind(creds);

                var byteArray = Encoding.ASCII.GetBytes($"{creds.Username}:{creds.Password}");

                client.BaseAddress = new Uri(baseUrls.PublicJustEatApis);
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(byteArray));

                client.DefaultRequestHeaders.Add("Accept-Tenant", "uk");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-GB");
                client.DefaultRequestHeaders.Add("Host", "public.je-apis.com");


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
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //app.

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
