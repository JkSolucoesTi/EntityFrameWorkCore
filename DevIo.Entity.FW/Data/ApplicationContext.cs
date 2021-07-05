using DevIo.Entity.FW.Data.Configurations;
using DevIo.Entity.FW.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevIo.Entity.FW.Data
{
    public class ApplicationContext : DbContext
    {
       private static readonly ILoggerFactory _logger = LoggerFactory.Create(prop => prop.AddConsole());
       public DbSet<Pedido> Pedidos { get; set; }
       public DbSet<Produto> Produtos { get; set; }
       public DbSet<Cliente> Clientes { get; set; }
       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging()
                .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database=EntityDevIO;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        }
    }
}
