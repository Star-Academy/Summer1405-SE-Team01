using System.Data.Common;
using Microsoft.Data.SqlClient;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;
using ASP.Net_core.Models;

namespace ASP.Net_core.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IConfiguration _configuration;

        public StudentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private (DbConnection connection, Compiler compiler) GetDatabaseSetup(string dbType)
        {
            if (string.IsNullOrWhiteSpace(dbType))
                throw new ArgumentException("پارامتر db الزامی است. از ?db=postgres یا ?db=sqlserver استفاده کنید.");

            if (dbType.Equals("postgres", StringComparison.OrdinalIgnoreCase))
            {
                var connectionString = _configuration.GetConnectionString("Postgres");
                if (string.IsNullOrEmpty(connectionString))
                    throw new InvalidOperationException("Connection String برای Postgres در appsettings.json تعریف نشده است.");

                return (new NpgsqlConnection(connectionString), new PostgresCompiler());
            }

            if (dbType.Equals("sqlserver", StringComparison.OrdinalIgnoreCase))
            {
                var connectionString = _configuration.GetConnectionString("SqlServer");
                if (string.IsNullOrEmpty(connectionString))
                    throw new InvalidOperationException("Connection String برای SQL Server در appsettings.json تعریف نشده است.");

                return (new SqlConnection(connectionString), new SqlServerCompiler());
            }

            throw new ArgumentException($"دیتابیس '{dbType}' پشتیبانی نمی‌شود. مقادیر مجاز: 'postgres' و 'sqlserver'");
        }

        public async Task<IEnumerable<Student>> GetAllAsync(string dbType)
        {
            var (connection, compiler) = GetDatabaseSetup(dbType);
            using (connection)
            {
                await connection.OpenAsync();
                var db = new QueryFactory(connection, compiler);

                // ✅ نام جدول با حروف کوچک
                return await db.Query("student2").GetAsync<Student>();
            }
        }

        public async Task<Student?> GetByStudentNumberAsync(string studentNumber, string dbType)
        {
            var (connection, compiler) = GetDatabaseSetup(dbType);
            using (connection)
            {
                await connection.OpenAsync();
                var db = new QueryFactory(connection, compiler);

                return await db.Query("student2")
                    .Where("studentnumber", studentNumber)   // ✅ ستون با حروف کوچک
                    .FirstOrDefaultAsync<Student>();
            }
        }

        public async Task<bool> CreateAsync(Student student, string dbType)
        {
            var (connection, compiler) = GetDatabaseSetup(dbType);
            using (connection)
            {
                await connection.OpenAsync();
                var db = new QueryFactory(connection, compiler);

                var affectedRows = await db.Query("student2").InsertAsync(new Dictionary<string, object>
                {
                    ["studentnumber"] = student.StudentNumber,
                    ["grade"] = student.Grade,
                    ["firstname"] = student.FirstName,
                    ["lastname"] = student.LastName,
                    ["ismale"] = student.IsMale,
                    ["dateofbirth"] = student.DateOfBirth,
                    ["leftunitscount"] = student.LeftUnitsCount
                });

                return affectedRows > 0;
            }
        }

        public async Task<bool> UpdateAsync(string studentNumber, Student student, string dbType)
        {
            var (connection, compiler) = GetDatabaseSetup(dbType);
            using (connection)
            {
                await connection.OpenAsync();
                var db = new QueryFactory(connection, compiler);

                var affectedRows = await db.Query("student2")
                    .Where("studentnumber", studentNumber)
                    .UpdateAsync(new Dictionary<string, object>
                    {
                        ["grade"] = student.Grade,
                        ["firstname"] = student.FirstName,
                        ["lastname"] = student.LastName,
                        ["ismale"] = student.IsMale,
                        ["dateofbirth"] = student.DateOfBirth,
                        ["leftunitscount"] = student.LeftUnitsCount
                    });

                return affectedRows > 0;
            }
        }

        public async Task<bool> DeleteAsync(string studentNumber, string dbType)
        {
            var (connection, compiler) = GetDatabaseSetup(dbType);
            using (connection)
            {
                await connection.OpenAsync();
                var db = new QueryFactory(connection, compiler);

                var affectedRows = await db.Query("student2")
                    .Where("studentnumber", studentNumber)
                    .DeleteAsync();

                return affectedRows > 0;
            }
        }
    }
}