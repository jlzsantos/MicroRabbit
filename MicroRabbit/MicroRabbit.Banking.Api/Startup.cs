using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MicroRabbit.Banking.Data;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Infra.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MicroRabbit.Banking.Api
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
            //services.AddDbContext<BankingDbContext>(options =>
            //{
            //    options.UseSqlServer(Configuration.GetConnectionString("BankingDbConnection"));
            //});

            services.AddDbContext<BankingDbContext>(ConfigureDatabaseOptions);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() 
                { 
                    Version = "v1",
                    Title = "Banking Microservice Rabbit API"
                });
            });

            services.AddMediatR(typeof(Startup));

            RegisterServices(services);
            
            services.AddControllers();

            //services.AddMvcCore()
            //    .AddApiExplorer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();       // lembrar de alterar 'launchUrl' em Properties\launchSettings.json
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking Microservice V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }

        private void ConfigureDatabaseOptions(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(Configuration.GetConnectionString("BankingDbConnection"), ConfigureSqlServerDatabaseOptions);
        }

        private void ConfigureSqlServerDatabaseOptions(SqlServerDbContextOptionsBuilder builder)
        {
            builder.MigrationsAssembly(typeof(BankingDbContextFactory).Namespace);
        }
    }
}
