using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace QuantityMeasurementApp.RepoLayer.Context
{
    public class DesignTimeDbContextFactory
        : IDesignTimeDbContextFactory<QuantityMeasurementDbContext>
    {
        public QuantityMeasurementDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<QuantityMeasurementDbContext>();

            // Uses env variable for migrations; falls back to a local postgres for dev
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                ?? "Host=localhost;Database=QuantityMeasurementDB;Username=postgres;Password=postgres";

            optionsBuilder.UseNpgsql(connectionString);

            return new QuantityMeasurementDbContext(optionsBuilder.Options);
        }
    }
}