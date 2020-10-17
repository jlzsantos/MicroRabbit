using MicroRabbit.Transfer.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MicroRabbit.Transfer.Data     // O name space tem que ser o nome do projeto 'MicroRabbit.Transfer.Data'
{
    public class TransferDbContextFactory : IDesignTimeDbContextFactory<TransferDbContext>
    {
        public TransferDbContextFactory()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public IConfiguration Configuration { get; }

        public TransferDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<TransferDbContext> builder = new DbContextOptionsBuilder<TransferDbContext>();

            builder.UseSqlServer(
                connectionString: Configuration.GetConnectionString("TransferDbConnection"),
                sqlServerOptionsAction: sqlOptions => sqlOptions.MigrationsAssembly(GetType().Namespace));

            return new TransferDbContext(builder.Options);
        }
    }
}
