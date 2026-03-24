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

            optionsBuilder.UseSqlServer(
                "Server=localhost\\SQLEXPRESS;Database=QuantityMeasurementDB;Trusted_Connection=True;TrustServerCertificate=True;");

            return new QuantityMeasurementDbContext(optionsBuilder.Options);
        }
    }
}
