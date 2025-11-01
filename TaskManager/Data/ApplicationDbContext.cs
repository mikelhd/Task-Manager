using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly SqlConnection _sqlConnection;
        public ApplicationDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _sqlConnection = new SqlConnection(configuration.GetConnectionString("ApplicationDBContext"));
        }
    }
}
