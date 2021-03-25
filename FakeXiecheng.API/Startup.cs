using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.API.Database;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace FakeXiecheng.API
{
     public class Startup
     {

         public IConfiguration Configuration { get; }

         public Startup(IConfiguration configuration)
         {
             Configuration = configuration;
         }

         // This method gets called by the runtime. Use this method to add services to the container.
          // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
          public void ConfigureServices(IServiceCollection services)
          {
            //添加注册身份认证的服务依赖
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            //注册身份认证服务
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => 
                    {
                        var secretByte = Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]);
                        options.TokenValidationParameters = new TokenValidationParameters() 
                        {
                            ValidateIssuer = true,
                            ValidIssuer = Configuration["Authentication:Issuer"],

                            ValidateAudience = true,
                            ValidAudience = Configuration["Authentication:Audience"],

                            ValidateLifetime = true,

                            IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                        };
                    });       

            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;  //对不支持的请求头进行错误处理
                
            }).AddXmlDataContractSerializerFormatters();   //添加对xml的输出、输出处理
            services.AddTransient<ITouristRouteRepository, TouristRouteRepository>();

              services.AddDbContext<AppDbContext>(option =>
              {
                  //option.UseSqlServer(Configuration["DbContext:ConnectionString"]);
                  option.UseMySql(Configuration["DbContext:MySQLConnectionString"]);
              });  //注入上下文关系对象

              //注入autoMapper依赖
              services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


          }  //ConfigureServices()

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                 app.UseDeveloperExceptionPage();
            }

            //where
            app.UseRouting();

            //who
            app.UseAuthentication();

            //权限验证
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // endpoints.MapGet("/", async context =>
                // {
                //     await context.Response.WriteAsync("Hello World!");
                //});
            });
        }
     }
}
