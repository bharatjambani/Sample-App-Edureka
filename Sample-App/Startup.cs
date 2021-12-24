using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Sample_App.Extensions;
using Sample_App.Models;
using Sample_App.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sample_App
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) //BJ: Built In Dependency Injection in .net Core
        {
            //services.AddControllers();
            services.AddControllersWithViews();// BJ: This will activate all the services required to return views along with Controller
            //services.AddSingleton<IRandomService, RandomService>();
            //services.AddSingleton<IRandomWrapper, RandomWrapper>();

            services.AddTransient<IRandomService, RandomService>();
            services.AddTransient<IRandomWrapper, RandomWrapper>();

            //services.AddScoped<IRandomService, RandomService>();
            //services.AddScoped<IRandomWrapper, RandomWrapper>();

            //services.AddScoped<IStoreRepo, ProductInMemoryRepo>(); // BJ: Get Data from Static class

            services.AddScoped<IStoreRepo, ProductSQLRepository>();

            //services.AddScoped<IStoreRepo, ProductInMemoryRepo>(); //BJ: In future we can change ProductInMemoryRepo to ProductInSQLRepo to get data from SQL database


            //BJ: IFileProvider : PhysicalFileProvider : Who has access to current dir
            IFileProvider physicalFileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            services.AddSingleton<IFileProvider>(physicalFileProvider);

            services.AddDbContext<BankOfAmericaContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:ProductConnection"]));

            //BJ: Activa Automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BankOfAmericaContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Request Pipeline
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
                if (env.IsStaging() || env.IsProduction())
            {
                app.UseExceptionHandler("/error");
            }
            app.UseStatusCodePages();//BJ: handle error code 400 to 599
            app.UseStaticFiles(); // exposes wwwroot folder

            //app.UseRequestCulture();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Current Culture set by Middle="+CultureInfo.CurrentCulture.DisplayName);
            //});

            //if below static files needs to be accessible to authenticated users
            //app.UseAuthentication();
            //use url : http://localhost:5000/files/demo.html to view this demo.html file
            //If static html files are kept somewhere else other than wwwroot
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider($"{env.ContentRootPath}/myStaticFiles"),
                RequestPath = "/files"
            }
                );

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("I'm first middleware");
            //    await next.Invoke();//calls the next middleware
            //});

            //Best Practise: Keep app.Run at the last
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Circuit Breaker");
            //});

            //Routing Middleware
            //selects the best endpoint for the current url
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //BJ:conventional Routing
            app.UseEndpoints(endpoints =>
            {
                //http://localhost/Chess
                endpoints.MapControllerRoute("category", "{category}/Page{productpage}", new { controller = "Home", action = "Index" });
                endpoints.MapControllerRoute("category", "{category}", new { controller = "Home", action = "Index", productpage = 1 });
                endpoints.MapControllerRoute("pagination", "Products/Page{productpage}", new { controller = "Home", action = "Index" });
                endpoints.MapGet("/authorize/{username:minlength(4)}", async context =>
                {
                    var username = context.Request.RouteValues["username"];
                    await context.Response.WriteAsync("Hello " + username);
                });
            });

            //configuration of the EndPoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute(); //Home is the default controller and Index will be action
            });


            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
