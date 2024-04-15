using Microsoft.EntityFrameworkCore;
using OasisAPI.Models;

namespace OasisAPI.Context;

public class OasisDbContext : DbContext
{
    public DbSet<OasisUser> OasisUsers { get; set; }
    public DbSet<OasisChat> OasisChats { get; set; }
    public DbSet<OasisMessage> OasisMessages { get; set; }
    
    public OasisDbContext(DbContextOptions<OasisDbContext> options) : base(options)
    {
        
    }
    
}