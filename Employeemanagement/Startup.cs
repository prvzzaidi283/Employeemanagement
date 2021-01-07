using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employeemanagement.Controllers;
using Employeemanagement.Middleware;
using Employeemanagement.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

namespace Employeemanagement
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
            

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest).AddJsonOptions(options=> {
                var resolver = options.SerializerSettings.ContractResolver;
                if(resolver!=null)
                {
                    (resolver as DefaultContractResolver).NamingStrategy = null;
                }
            });
            services.AddScoped<ILogger, Logger<EmployeeController>>();
            services.AddDbContext<EmployeeContext>(options => options.UseSqlServer(Configuration.GetConnectionString("devconnection")));
            services.AddCors();

            services.AddDefaultIdentity<ApplicationUser>().AddEntityFrameworkStores<EmployeeContext>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            }
           );
            /////////jwt token generation/////
            ///
            /// 
            /// 
            /// 
            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());
            services.AddAuthentication(x => {

                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;


            }).AddJwtBearer(x => {
            x.RequireHttpsMetadata = false;
            x.SaveToken = false;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero



                };
            
            
            });
            ///ConfigureTraceSourceLogging();jwt
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }
        //    loggerFactory.AddFile(Configuration.GetSection("Logging"));
        //    //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
        //    loggerFactory.AddDebug();
        //    app.UseRequestResponseLogging();

        //    app.UseCors(options => options.WithOrigins("http://localhost:4200")
        //  .AllowAnyMethod()
        //  .AllowAnyHeader());
        //    app.UseMvc();
        //}
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddFile(Configuration.GetSection("Logging"));
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseCors(builder =>
            builder.WithOrigins(Configuration["ApplicationSettings:Client_URL"].ToString())
            .AllowAnyHeader()
            .AllowAnyMethod()

            );

            app.UseAuthentication();

            app.UseMvc();
        }

    }
}
