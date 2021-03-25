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
            //���ע�������֤�ķ�������
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            //ע�������֤����
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
                setupAction.ReturnHttpNotAcceptable = true;  //�Բ�֧�ֵ�����ͷ���д�����
                
            }).AddXmlDataContractSerializerFormatters();   //��Ӷ�xml��������������
            services.AddTransient<ITouristRouteRepository, TouristRouteRepository>();

              services.AddDbContext<AppDbContext>(option =>
              {
                  //option.UseSqlServer(Configuration["DbContext:ConnectionString"]);
                  option.UseMySql(Configuration["DbContext:MySQLConnectionString"]);
              });  //ע�������Ĺ�ϵ����

              //ע��autoMapper����
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

            //Ȩ����֤
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
