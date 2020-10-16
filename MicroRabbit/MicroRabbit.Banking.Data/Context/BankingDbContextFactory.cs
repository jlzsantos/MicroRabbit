using MicroRabbit.Banking.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MicroRabbit.Banking.Data  // O name space tem que ser o nome do projeto 'MicroRabbit.Banking.Data'
{
    public class BankingDbContextFactory : IDesignTimeDbContextFactory<BankingDbContext>
    {
        public BankingDbContextFactory()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public IConfiguration Configuration { get; }

        public BankingDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<BankingDbContext> builder = new DbContextOptionsBuilder<BankingDbContext>();

            builder.UseSqlServer(
                connectionString: Configuration.GetConnectionString("BankingDbConnection"),
                sqlServerOptionsAction: sqlOptions => sqlOptions.MigrationsAssembly(GetType().Namespace));

            return new BankingDbContext(builder.Options);
        }
    }
}
