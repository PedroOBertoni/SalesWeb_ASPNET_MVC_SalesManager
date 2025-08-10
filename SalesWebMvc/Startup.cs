using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 

using SalesWebMvc.Data;
using SalesWebMvc.Services; 
using Microsoft.EntityFrameworkCore;

using System;
using SalesWebMvc.Models;

namespace SalesWebMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Adiciona serviços ao contêiner
        public void ConfigureServices(IServiceCollection services)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            // Lê usuário e senha do ambiente (Render)
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPass = Environment.GetEnvironmentVariable("DB_PASS");

            // Monta a connection string usando variáveis
            var connectionString =
                $"Host=ep-broad-dawn-a8lwicsl-pooler.eastus2.azure.neon.tech;" +
                $"Database=saleswebmvcappdb;" +
                $"Username={dbUser};" +
                $"Password={dbPass};" +
                $"Ssl Mode=Require;Trust Server Certificate=true";

            services.AddDbContext<SalesWebMvcContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<SellerService>();
            services.AddScoped<DepartmentService>();
            services.AddScoped<SalesRecordService>();

            services.AddControllersWithViews();
        }

        // Configura o pipeline HTTP
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
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
