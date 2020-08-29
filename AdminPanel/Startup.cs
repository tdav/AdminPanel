using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AdminPanel.Models;
using AdminPanel.Database;
using Microsoft.EntityFrameworkCore;
using AdminPanel.Database.Core;
using Microsoft.OpenApi.Models;

namespace AdminPanel
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(MetaData));
            services.AddScoped(typeof(Repository<>));

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MyDbContext>(options =>options.UseSqlite(connection));


            services.AddControllersWithViews(o => o.Conventions.Add(new GenericControllerRouteConvention())).
                ConfigureApplicationPartManager(m => m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider())); 
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
