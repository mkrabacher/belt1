using Microsoft.EntityFrameworkCore;
 
namespace belt1.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Activity> activities { get; set; }
        public DbSet<UserActivity> useractivities { get; set; }
    }
}
