using Npgsql;
using QuantityMeasurementApp.Exception;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using QuantityMeasurementApp.RepoLayer.Utilities;

namespace QuantityMeasurementApp.RepoLayer.Persistence
{
    public class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private readonly string _connectionString;

        public QuantityMeasurementDatabaseRepository(bool useTestDb = false)
        {
            var config = ApplicationConfig.GetInstance();
            _connectionString = useTestDb
                ? config.GetTestConnectionString()
                : config.GetConnectionString();

            InitializeSchema();
        }

        private void InitializeSchema()
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();

                string sql = @"
                    CREATE TABLE IF NOT EXISTS quantity_measurement_entity (
                        id           VARCHAR(50)   NOT NULL PRIMARY KEY,
                        operation    VARCHAR(50)   NOT NULL,
                        operand_one  VARCHAR(200)  NULL,
                        operand_two  VARCHAR(200)  NULL,
                        result       VARCHAR(200)  NULL,
                        measure_type VARCHAR(50)   NULL,
                        created_at   TIMESTAMP     DEFAULT NOW()
                    )";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                throw new DatabaseException("Failed to initialize database schema.", ex);
            }
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();

                var sql = @"INSERT INTO quantity_measurement_entity
                            (id, operation, operand_one, operand_two, result, measure_type)
                            VALUES (@id, @op, @o1, @o2, @res, @mt)";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id",  entity.Id.ToString());
                cmd.Parameters.AddWithValue("@op",  entity.Operation);
                cmd.Parameters.AddWithValue("@o1",  entity.OperandOne);
                cmd.Parameters.AddWithValue("@o2",  entity.OperandTwo);
                cmd.Parameters.AddWithValue("@res", entity.Result);
                cmd.Parameters.AddWithValue("@mt",  entity.MeasureType);
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                throw new DatabaseException("Failed to save entity.", ex);
            }
        }

        public List<QuantityMeasurementEntity> GetAll()
        {
            var list = new List<QuantityMeasurementEntity>();
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();

                var sql = @"SELECT id, operation, operand_one, operand_two, result, measure_type
                            FROM quantity_measurement_entity
                            ORDER BY created_at DESC";

                using var cmd    = new NpgsqlCommand(sql, conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(MapRow(reader));
            }
            catch (System.Exception ex)
            {
                throw new DatabaseException("Failed to retrieve all measurements.", ex);
            }
            return list;
        }

        public List<QuantityMeasurementEntity> GetByOperation(string operation)
        {
            var list = new List<QuantityMeasurementEntity>();
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();

                var sql = @"SELECT id, operation, operand_one, operand_two, result, measure_type
                            FROM quantity_measurement_entity
                            WHERE operation = @op
                            ORDER BY created_at DESC";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@op", operation.ToUpper());
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(MapRow(reader));
            }
            catch (System.Exception ex)
            {
                throw new DatabaseException($"Failed to get by operation: {operation}", ex);
            }
            return list;
        }

        public List<QuantityMeasurementEntity> GetByMeasureType(string measureType)
        {
            var list = new List<QuantityMeasurementEntity>();
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();

                var sql = @"SELECT id, operation, operand_one, operand_two, result, measure_type
                            FROM quantity_measurement_entity
                            WHERE measure_type = @mt
                            ORDER BY created_at DESC";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mt", measureType.ToUpper());
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(MapRow(reader));
            }
            catch (System.Exception ex)
            {
                throw new DatabaseException($"Failed to get by measure type: {measureType}", ex);
            }
            return list;
        }

        public List<QuantityMeasurementEntity> GetFullHistory()
        {
            var list = new List<QuantityMeasurementEntity>();
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();

                var sql = @"SELECT entity_id, operation, operand_one, operand_two,
                                   result, measure_type
                            FROM quantity_measurement_history
                            ORDER BY action_at DESC";

                using var cmd    = new NpgsqlCommand(sql, conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(MapRow(reader));
            }
            catch (System.Exception ex)
            {
                throw new DatabaseException("Failed to retrieve full history.", ex);
            }
            return list;
        }

        public int GetTotalCount()
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();

                using var cmd = new NpgsqlCommand(
                    "SELECT COUNT(*) FROM quantity_measurement_entity", conn);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (System.Exception ex)
            {
                throw new DatabaseException("Failed to get total count.", ex);
            }
        }

        public void DeleteAll()
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();

                using var cmd1 = new NpgsqlCommand(
                    "DELETE FROM quantity_measurement_history", conn);
                cmd1.ExecuteNonQuery();

                using var cmd2 = new NpgsqlCommand(
                    "DELETE FROM quantity_measurement_entity", conn);
                cmd2.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                throw new DatabaseException("Failed to delete all records.", ex);
            }
        }

        public string GetPoolStats()
        {
            int count = GetTotalCount();
            return $"PostgreSQL | Total records: {count} | DB: QuantityMeasurementDB";
        }

        private static QuantityMeasurementEntity MapRow(NpgsqlDataReader reader)
        {
            return new QuantityMeasurementEntity(
                reader.GetString(1),
                reader.IsDBNull(2) ? "" : reader.GetString(2),
                reader.IsDBNull(3) ? "" : reader.GetString(3),
                reader.IsDBNull(4) ? "" : reader.GetString(4),
                reader.IsDBNull(5) ? "" : reader.GetString(5)
            )
            { Id = Guid.Parse(reader.GetString(0)) };
        }
    }
}