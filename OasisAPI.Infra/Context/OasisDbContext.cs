using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace OasisAPI.Infra.Context;

public class OasisDbContext : DbContext
{
    public DbSet<OasisUser> OasisUsers { get; set; }
    public DbSet<OasisChat> OasisChats { get; set; }
    public DbSet<OasisChatBotDetails> OasisChatBotsDetails { get; set; }
    public DbSet<OasisMessage> OasisMessages { get; set; }
    
    public OasisDbContext(DbContextOptions<OasisDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OasisUser>().ToTable("oasis_users");
        modelBuilder.Entity<OasisChat>().ToTable("oasis_chats");
        modelBuilder.Entity<OasisChatBotDetails>().ToTable("oasis_chat_bot_details");
        modelBuilder.Entity<OasisMessage>().ToTable("oasis_messages");
    }
}