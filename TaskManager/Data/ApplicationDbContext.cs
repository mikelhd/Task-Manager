using Microsoft.EntityFrameworkCore;

namespace TaskManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly SqlConnection
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
