using Microsoft.EntityFrameworkCore;
    public class MvcTextContext : DbContext
    {
        public MvcTextContext (DbContextOptions<MvcTextContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        public DbSet<mvc.Models.Text> Reviews { get; set; }
        public DbSet<mvc.Models.Employee> Team { get; set; }
        public DbSet<mvc.Models.SystemUser> SystemUsers { get; set; }
    }