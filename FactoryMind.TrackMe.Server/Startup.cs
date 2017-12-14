using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Business.Repos;
using FactoryMind.TrackMe.Business.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FactoryMind.TrackMe.Domain.Models;
using FactoryMind.TrackMe.Server.Controllers;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace FactoryMind.TrackMe.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions((Settings) =>
                {
                    Settings.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();// usa camelcase 
                    Settings.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;                   // non scrive valori nulli
                    Settings.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore; 
                });
            services.AddScoped<IAuthorizationContext, AuthorizationContext>();
            services.AddScoped<IPositionReposotory, DbPositionRepository>();
            services.AddScoped<IRoomRepository, DbRoomRepository>();
            services.AddScoped<IUserRepository, DbUserRepository>();
            services.AddScoped<AuthenticationService>();
            services.AddScoped<UserService>();
            services.AddScoped<MapService>();
            services.AddScoped<PositionService>();
            services.AddScoped<RoomService>();
            services.AddScoped<DataBaseContext>();
            //services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseCors(cors =>
                            {
                                cors.AllowAnyHeader();
                                cors.AllowAnyOrigin();
                                cors.AllowAnyMethod();
                            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseDefaultFiles();
            app.UseMiddleware(typeof(AuthorizationMiddleware));
            app.UseMvc();

        }
    }
}
