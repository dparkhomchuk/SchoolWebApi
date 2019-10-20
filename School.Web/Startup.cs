﻿using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using School.Database;
using School.Database.UnitOfWork;
using School.Interface;
using School.Service;
using School.UnitOfWork;

//using Microsoft.OpenApi.Models;

namespace School.Web
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
            services.AddDbContext<SchoolDataBase>(option =>
            {
                option.UseMySQL(Configuration.GetSection("ConnectionString").Value);
                option.UseLazyLoadingProxies();
            });

            services.AddScoped<IUnitOfWork, EFUnitOfWork>();

            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IStudentService, StudentService>();


            IMapper mapper = Mapping.GetMapper();
            services.AddSingleton(mapper);


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo(){Title = "School API",Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        //.AllowCredentials()
                );
            });
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc((options) => options.EnableEndpointRouting = false);
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            env.EnvironmentName = EnvironmentName.Development;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appError =>
                {
                    app.Run(async context =>
                    {
                        context.Response.StatusCode = 400;
                        var exception = context.Features.Get<IExceptionHandlerPathFeature>();

                        byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(exception.Error.Message);
                        context.Response.Body.Write(bytes);
                    });
                });
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "School API V1");
            });

            //app.UseCors(opt => opt.AllowAnyOrigin());
            app.UseMvc();
            app.UseCors("CorsPolicy");
            app.UseHealthChecks("/health");
        }
    }
}
