
using CRUDAPI.Data;
using CRUDAPI.DTOs.Employees;
using CRUDAPI.Errors;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CRUDAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IValidator<CreateEmployeeDto>, CreateEmployeeDtoValidation>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddDbContext<ApplicationDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddSwaggerGen();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            //var allowedOrigins = builder.Configuration.GetSection("AllowOrigins").Get<string[]>();
            //builder.Services.AddCors(
            //    options =>
            //    {
            //        options.AddPolicy("AllowAll",
            //            builder =>
            //            {
            //                builder.AllowAnyMethod().AllowAnyHeader().WithOrigins(allowedOrigins);
            //            });
            //        options.AddPolicy("policy",
            //            builder =>
            //            {
            //                builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
            //            });
            //    });
            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("AllowAll");

            app.MapControllers();

            app.UseExceptionHandler(opt => { });

            app.Run();
        }
    }
}
