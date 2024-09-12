using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace OasisAPI.Infra.Context;

public class OasisDbContext : DbContext
{
    public DbSet<OasisUser> OasisUsers { get; set; }
    public DbSet<OasisChat> OasisChats { get; set; }
    public DbSet<OasisChatBotInfo> OasisChatBotInfos { get; set; }
    public DbSet<OasisMessage> OasisMessages { get; set; }
    
    public OasisDbContext(DbContextOptions<OasisDbContext> options) : base(options)
    {
        
    }
    
}