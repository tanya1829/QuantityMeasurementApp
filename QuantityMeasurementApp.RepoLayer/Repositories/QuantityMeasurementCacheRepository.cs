using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuantityMeasurementApp.RepoLayer.Repositories
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static QuantityMeasurementCacheRepository? _instance;

        private readonly List<QuantityMeasurementEntity> _cache;

        // Use the executable's directory — always resolves correctly
        // regardless of where dotnet run is called from
        private static readonly string FilePath = GetFilePath();

        private static string GetFilePath()
        {
            // Walk up from bin/Debug/net10.0 to find the solution root
            // and place measurements.json at the RepoLayer project level
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string path   = Path.Combine(exeDir, "measurements.json");
            return path;
        }

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            Converters    = { new JsonStringEnumConverter() }
        };

        private QuantityMeasurementCacheRepository()
        {
            Console.WriteLine($"[INFO] JSON file location: {FilePath}");

            // Immediately test that we can write to the path
            EnsureFileExists();

            _cache = LoadFromFile();
            Console.WriteLine($"[INFO] Loaded {_cache.Count} existing record(s) from JSON.\n");
        }

        public static QuantityMeasurementCacheRepository GetInstance()
        {
            _instance ??= new QuantityMeasurementCacheRepository();
            return _instance;
        }

        // ── IQuantityMeasurementRepository ───────────────────────────────

        public void Save(QuantityMeasurementEntity entity)
        {
            _cache.Add(entity);
            PersistToFile();
        }

        public List<QuantityMeasurementEntity> GetAll() =>
            _cache.ToList();

        public List<QuantityMeasurementEntity> GetByOperation(string operation) =>
            _cache.Where(e => e.Operation == operation.ToUpper()).ToList();

        public List<QuantityMeasurementEntity> GetByMeasureType(string measureType) =>
            _cache.Where(e => e.MeasureType == measureType.ToUpper()).ToList();

        public List<QuantityMeasurementEntity> GetFullHistory() =>
            _cache.ToList();

        public int GetTotalCount() =>
            _cache.Count;

        public void DeleteAll()
        {
            _cache.Clear();
            PersistToFile();
        }

        public string GetPoolStats() =>
            $"Cache Repository (JSON) | Total records: {_cache.Count} | File: {FilePath}";

        // ── JSON persistence helpers ──────────────────────────────────────

        /// <summary>
        /// Creates an empty JSON array file if it does not exist yet.
        /// Also verifies we have write permission to this location.
        /// </summary>
        private static void EnsureFileExists()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    File.WriteAllText(FilePath, "[]");
                    Console.WriteLine("[INFO] Created new measurements.json file.");
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"[ERROR] Cannot create measurements.json at: {FilePath}");
                Console.WriteLine($"[ERROR] Reason: {ex.Message}");
            }
        }

        private static List<QuantityMeasurementEntity> LoadFromFile()
        {
            if (!File.Exists(FilePath))
                return new List<QuantityMeasurementEntity>();

            try
            {
                string json = File.ReadAllText(FilePath);
                if (string.IsNullOrWhiteSpace(json) || json.Trim() == "[]")
                    return new List<QuantityMeasurementEntity>();

                var records = JsonSerializer.Deserialize<List<EntityJsonRecord>>(json, JsonOptions)
                              ?? new List<EntityJsonRecord>();

                return records.Select(r => new QuantityMeasurementEntity(
                    r.Operation,
                    r.OperandOne,
                    r.OperandTwo,
                    r.Result,
                    r.MeasureType)
                { Id = r.Id }).ToList();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"[WARN] Could not read measurements.json (starting fresh): {ex.Message}");
                return new List<QuantityMeasurementEntity>();
            }
        }

        private void PersistToFile()
        {
            try
            {
                var records = _cache.Select(e => new EntityJsonRecord
                {
                    Id          = e.Id,
                    Operation   = e.Operation,
                    OperandOne  = e.OperandOne,
                    OperandTwo  = e.OperandTwo,
                    Result      = e.Result,
                    MeasureType = e.MeasureType
                }).ToList();

                string json = JsonSerializer.Serialize(records, JsonOptions);
                File.WriteAllText(FilePath, json);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to write measurements.json: {ex.Message}");
                Console.WriteLine($"[ERROR] Path attempted: {FilePath}");
            }
        }

        // ── Private DTO for JSON serialization ───────────────────────────

        private class EntityJsonRecord
        {
            public Guid   Id          { get; set; }
            public string Operation   { get; set; } = "";
            public string OperandOne  { get; set; } = "";
            public string OperandTwo  { get; set; } = "";
            public string Result      { get; set; } = "";
            public string MeasureType { get; set; } = "";
        }
    }
}