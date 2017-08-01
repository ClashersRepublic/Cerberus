using Magic.ClashOfClans.Database;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Magic.Database
{
    internal class ucsdbEntities : DbContext
    {
        public ucsdbEntities(string connectionString) : base("name=" + connectionString)
        {
            // Space
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
        
        public virtual DbSet<clan> clan { get; set; }
        public virtual DbSet<player> player { get; set; }
    }
}
