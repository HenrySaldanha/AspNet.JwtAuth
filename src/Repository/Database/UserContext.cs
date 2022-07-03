using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Database;
public class UserContext : DbContext
{
    public UserContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}
