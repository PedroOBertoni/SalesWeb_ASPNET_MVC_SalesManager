using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http; // Necessário para CookiePolicy
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc; // Necessário para AddControllersWithViews
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; // Necessário para IWebHostEnvironment

using SalesWebMvc.Data; // Para o seu DbContext (assumindo que o nome é SalesWebMvcContext)
using SalesWebMvc.Services; // Para SellerService e DepartmentService
using Microsoft.EntityFrameworkCore; // Para o método UseNpgsql (ou UseSqlServer, etc.)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;

using Npgsql;

namespace SalesWebMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Este método é usado para adicionar serviços ao contêiner de injeção de dependência.
        public void ConfigureServices(IServiceCollection services)
        {

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); // <--- LINHA CHAVE ADICIONADA

            services.AddDbContext<SalesWebMvcContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("SalesWebMvcContext")));

            services.AddScoped<SellerService>();
            services.AddScoped<DepartmentService>();
            services.AddScoped<SalesRecordService>();

            services.AddControllersWithViews(); // Adiciona suporte para controladores MVC com Views
        }

        // Este método é usado para configurar o pipeline de requisições HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) // Use IWebHostEnvironment
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

            app.UseHttpsRedirection(); // Redireciona requisições HTTP para HTTPS
            app.UseStaticFiles(); // Habilita o serviço de arquivos estáticos (CSS, JS, imagens)
            app.UseCookiePolicy(); // Habilita o middleware de política de cookies (se você tiver _CookieConsentPartial)

            app.UseRouting(); // Define o roteamento para as requisições

            app.UseAuthorization(); // Habilita a autorização (se você tiver)

            app.UseEndpoints(endpoints =>
            {
                // Mapeia os endpoints dos controladores MVC.
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}