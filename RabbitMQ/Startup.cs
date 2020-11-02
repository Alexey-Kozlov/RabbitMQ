using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using RabbitMQ.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using MediatR;
using RabbitMQ.Commands;
using AKDbHelpers.Helpers;
using RabbitMQ.Models;
using RabbitMQ.Handlers;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using RabbitMQ.Controllers;

namespace RabbitMQ
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RabbitOptions>(Configuration.GetSection("RabbitMq"));
            var rabbitOptions = Configuration.GetSection("RabbitMq").Get<RabbitOptions>();
            services.AddSingleton<IQueueProvider, RabbitMqProvider>(provider =>
            {
                var credentials = new MqCredentials(rabbitOptions.Credentials.HostName, rabbitOptions.Credentials.UserName, 
                    rabbitOptions.Credentials.Password, rabbitOptions.Queues);

                var rabbitProvider = new RabbitMqProvider(credentials, rabbitOptions.Queues, rabbitOptions.ExchangeName);
                rabbitProvider.Bind();

                //rabbitProvider.Subscribe(ReceiveMessage.GetMesAuth, "auth");
                //rabbitProvider.Subscribe(ReceiveMessage.GetMesAuth2, "auth2");

                return rabbitProvider;
            });
            services.AddSingleton<IRabbitService>(new RabbitService(p =>
            { 
                p.Credentials = rabbitOptions.Credentials;
                p.Queues = rabbitOptions.Queues;
                p.ExchangeName = rabbitOptions.ExchangeName;
            }));
            services.AddScoped<ITestRepository, TestReporitory>();
            services.AddControllers();

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //инициализация autofac
            builder.RegisterModule(new MediatorModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
