using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using School.Database;
using School.Database.UnitOfWork;
using School.Interface;
using School.Service;
using School.UnitOfWork;
using Swashbuckle.AspNetCore.Swagger;

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
                option.UseSqlServer(Configuration.GetSection("ConnectionString").Value);
                //option.UseSqlServer(configuration.GetSection("ConnectionString").Value);
                option.UseLazyLoadingProxies();
            });

            services.AddScoped<IUnitOfWork, EFUnitOfWork>();

            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IStudentService, StudentService>();


            IMapper mapper = Mapping.GetMapper();
            services.AddSingleton(mapper);


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info(){Title = "School API",Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                );
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "School API V1");
            });

            //app.UseCors(opt => opt.AllowAnyOrigin());
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseCors("CorsPolicy");
        }
    }
}
