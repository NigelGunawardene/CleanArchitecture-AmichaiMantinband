using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Domain.MenuAggregate;
using Microsoft.EntityFrameworkCore;

namespace BuberDinner.Infrastructure.Persistence;
public class BuberDinnerDbContext : DbContext
{
    public BuberDinnerDbContext(DbContextOptions<BuberDinnerDbContext> options) : base(options)
    {
    }

    public DbSet<Menu> Menus { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BuberDinnerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        // this is the method where you can iterate over all your entities and apply a configuration
        // For example if you want to tell it to never generate Ids for PK, you can do
        //modelBuilder.Model.GetEntityTypes()
        //    .SelectMany(e => e.GetProperties())
        //    .Where(p => p.IsPrimaryKey())
        //    .ToList()
        //    .ForEach(p => p.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.Never);
    }
}
