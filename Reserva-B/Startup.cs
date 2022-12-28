using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NT1_2022_1C_B_G2.Data;
using NT1_2022_1C_B_G2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NT1_2022_1C_B_G2
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
            // configuracion DbContext in memory
            //services.AddDbContext<ReservasContext>(options => options.UseInMemoryDatabase("ReservasDb"));

            //configuracion DbContext SqLite
            //services.AddDbContext<ReservasContext>(options => options.UseSqlite("DataSource = ReservasDb"));

            #region Identity
            services.AddIdentity<Persona, Rol>().AddEntityFrameworkStores<ReservasContext>();
            services.Configure<IdentityOptions>(
                opciones =>
                {
                    opciones.Password.RequiredLength = 2;
                    //opciones.Password.RequireDigit = true;
                    //opciones.Password.RequireLowercase = true;
                    //opciones.Password.RequireNonAlphanumeric = true;
                    //opciones.Password.RequireUppercase = true;
                }
                );
            #endregion

            //configuracion DbContext SqlServer
            services.AddDbContext<ReservasContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ReservasCS")));

            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, opciones =>
            {
                opciones.LoginPath = "/Account/IniciarSesion";
                opciones.AccessDeniedPath = "/Account/AccesoDenegado";
            });


            services.AddControllersWithViews();
        }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ReservasContext reservasContext)
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

            reservasContext.Database.Migrate();

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
