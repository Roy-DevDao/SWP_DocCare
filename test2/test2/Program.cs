using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using test2.DAO;
using test2.Data;

namespace test2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<DocCareContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DocCare"));
            });

            builder.Services.AddAuthentication("MyCookieAuth")
            .AddCookie("MyCookieAuth", options =>
            {
                options.Cookie.Name = "MyAuthCookie";
                options.LoginPath = "/Home/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<PatientDao>();
            builder.Services.AddScoped<DoctorDAO>();
            builder.Services.AddScoped<AppointmentDAO>();
            builder.Services.AddScoped<FeedbackDAO>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.MapControllers();
            app.UseAuthorization();

            // Default route for Home controller
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Route for StaffController1
            app.MapControllerRoute(
               name: "staff",
               pattern: "{controller=Staff}/{action=AppoitmentList}/{id?}");

            app.MapControllerRoute(
               name: "doctor",
               pattern: "{controller=Doctor}/{action=VIewAppointment}/{id?}");

            app.MapControllerRoute(
               name: "patient",
               pattern: "{controller=Patient}/{action=AppointmentHistory}/{id?}");
            app.MapControllerRoute(
              name: "admin",
              pattern: "{controller=Admin}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
