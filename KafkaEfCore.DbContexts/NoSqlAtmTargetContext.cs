using KafkaEfCore.DbContexts.NoSqlModel;
using KafkaEFCore.Producer.Implementations;
using Microsoft.EntityFrameworkCore;
namespace KafkaEfCore.DbContexts;

public partial class NoSqlAtmTargetContext : KafkaDbContextBase
{
    public NoSqlAtmTargetContext(DbContextOptions<NoSqlAtmTargetContext> options)
        : this(options as DbContextOptions)
    {
    }
    protected NoSqlAtmTargetContext(DbContextOptions dbContextOptions):base(dbContextOptions)
    {
        Database.EnsureCreated();
    }
    protected NoSqlAtmTargetContext()
    {
        
    }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<AssetLabel> AssetLabels { get; set; }
    public DbSet<AssetTrack> AssetTracks { get; set; }
    public DbSet<AudioInfo> AudioInfos { get; set; }
    public DbSet<AudioLabel> AudioLabels { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AssetLabel>(e =>
        {
            e.HasKey(e => e.Name);
        });
        modelBuilder.Entity<AudioLabel>(e =>
        {
            e.HasKey(e => e.Name);
        });
    }

}
