using Microsoft.EntityFrameworkCore;
using MVC.Data.Data;
using MVC.Domain.Services;
using MVC.Domain.Services.Interfaces;

namespace VeterinariaMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            #region SQL SERVER
            builder.Services.AddDbContext<VeterinariaContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("ConexionVeterinaria"));

            });
            #endregion

            #region Inyeccion de dependencias
            builder.Services.AddScoped<IPropietarioServices, PropietarioServices>();
            builder.Services.AddScoped<IMascotaServices, MascotaServices>();
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
