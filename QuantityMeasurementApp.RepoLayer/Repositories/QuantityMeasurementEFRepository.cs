using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Data;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.RepoLayer.Repositories
{
    public class QuantityMeasurementEfRepository : IQuantityMeasurementRepository
    {
        private readonly QuantityMeasurementDbContext _context;

        public QuantityMeasurementEfRepository(QuantityMeasurementDbContext context)
        {
            _context = context;
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            _context.QuantityMeasurements.Add(entity);
            _context.SaveChanges();
        }

        public List<QuantityMeasurementEntity> GetAll() =>
            _context.QuantityMeasurements
                .OrderByDescending(e => e.Id)
                .ToList();

        public List<QuantityMeasurementEntity> GetByOperation(string operation) =>
            _context.QuantityMeasurements
                .Where(e => e.Operation == operation.ToUpper())
                .OrderByDescending(e => e.Id)
                .ToList();

        public List<QuantityMeasurementEntity> GetByMeasureType(string measureType) =>
            _context.QuantityMeasurements
                .Where(e => e.MeasureType == measureType.ToUpper())
                .OrderByDescending(e => e.Id)
                .ToList();

        public List<QuantityMeasurementEntity> GetFullHistory() =>
            _context.QuantityMeasurements
                .OrderByDescending(e => e.Id)
                .ToList();

        public int GetTotalCount() =>
            _context.QuantityMeasurements.Count();

        public void DeleteAll()
        {
            _context.QuantityMeasurements.RemoveRange(
                _context.QuantityMeasurements);
            _context.SaveChanges();
        }

        public string GetPoolStats()
        {
            int count = GetTotalCount();
            return $"EF Core SQL Server | Total records: {count}";
        }
    }
}