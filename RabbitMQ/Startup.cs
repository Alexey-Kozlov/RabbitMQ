using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

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
                    rabbitOptions.Credentials.Password, rabbitOptions.QueueName);

                var rabbitProvider = new RabbitMqProvider(credentials, rabbitOptions.QueueName);
                rabbitProvider.Bind();

                //rabbitProvider.Subscribe(ReceiveMessage.GetMes);

                return rabbitProvider;
            });
            services.AddSingleton<IRabbitService>(new RabbitService(p =>
            { 
                p.Credentials = rabbitOptions.Credentials;
                p.AutoDelete = rabbitOptions.AutoDelete;
                p.QueueName = rabbitOptions.QueueName;
            }));



            services.AddControllers();
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
