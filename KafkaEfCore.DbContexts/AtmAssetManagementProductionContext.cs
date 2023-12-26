using System;
using System.Collections.Generic;
using KafkaEfCore.DbContexts.Models;
using KafkaEFCore.Producer.Implementations;
using Microsoft.EntityFrameworkCore;
namespace KafkaEfCore.DbContexts;

public partial class AtmAssetManagementProductionContext : KafkaDbContextBase
{
    public AtmAssetManagementProductionContext(DbContextOptions<AtmAssetManagementProductionContext> options)
        : this(options as DbContextOptions)
    {
    }
    protected AtmAssetManagementProductionContext(DbContextOptions dbContextOptions):base(dbContextOptions)
    {
        
    }
    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<AssetLabel> AssetLabels { get; set; }

    public virtual DbSet<AssetTrack> AssetTracks { get; set; }

    public virtual DbSet<AudioInfo> AudioInfos { get; set; }

    public virtual DbSet<AudioLabel> AudioLabels { get; set; }

    public virtual DbSet<BusinessInfo> BusinessInfos { get; set; }

    public virtual DbSet<ChannelGroupType> ChannelGroupTypes { get; set; }

    public virtual DbSet<ChannelNetwork> ChannelNetworks { get; set; }

    public virtual DbSet<ChannelSync> ChannelSyncs { get; set; }

    public virtual DbSet<ContactInfo> ContactInfos { get; set; }

    public virtual DbSet<ContractInfomation> ContractInfomations { get; set; }

    public virtual DbSet<ContractOption> ContractOptions { get; set; }

    public virtual DbSet<ContractType> ContractTypes { get; set; }

    public virtual DbSet<DeliveryDetail> DeliveryDetails { get; set; }

    public virtual DbSet<DeviceCode> DeviceCodes { get; set; }

    public virtual DbSet<FolderDriveMap> FolderDriveMaps { get; set; }

    public virtual DbSet<JoinNetwork> JoinNetworks { get; set; }

    public virtual DbSet<JoinNetworkHistory> JoinNetworkHistories { get; set; }

    public virtual DbSet<MediaAuthor> MediaAuthors { get; set; }

    public virtual DbSet<MediaChannel> MediaChannels { get; set; }

    public virtual DbSet<MediaDelivery> MediaDeliveries { get; set; }

    public virtual DbSet<MediaNetwork> MediaNetworks { get; set; }

    public virtual DbSet<MediaRepositoriesAllowed> MediaRepositoriesAlloweds { get; set; }

    public virtual DbSet<MediaRepository> MediaRepositories { get; set; }

    public virtual DbSet<MediaRepositoryAsset> MediaRepositoryAssets { get; set; }

    public virtual DbSet<MetaFile> MetaFiles { get; set; }

    public virtual DbSet<MetaFolder> MetaFolders { get; set; }

    public virtual DbSet<NamingFolder> NamingFolders { get; set; }

    public virtual DbSet<PaymentInfo> PaymentInfos { get; set; }

    public virtual DbSet<PersistedGrant> PersistedGrants { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<ReportChannel> ReportChannels { get; set; }

    public virtual DbSet<ReportView> ReportViews { get; set; }

    public virtual DbSet<RouteNoteAlbum> RouteNoteAlbums { get; set; }

    public virtual DbSet<RouteNoteAudio> RouteNoteAudios { get; set; }

    public virtual DbSet<VideoPrice> VideoPrices { get; set; }

    public virtual DbSet<Whitelist> Whitelists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.Property(e => e.RoleId).HasMaxLength(450);

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.SigningDate).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");
            entity.Property(e => e.UserCreatorId).HasMaxLength(450);
            entity.Property(e => e.UserName).HasMaxLength(256);


            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasIndex(e => e.DateCreated, "IX_Assets_DateCreated");

            entity.HasIndex(e => e.LabelName, "IX_Assets_LabelName");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");

            entity.HasOne(d => d.LabelNameNavigation).WithMany(p => p.Assets).HasForeignKey(d => d.LabelName);

            entity.HasOne(d => d.Media).WithMany(p => p.Assets)
                .HasForeignKey(d => d.MediaId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<AssetLabel>(entity =>
        {
            entity.HasKey(e => e.Name);

            entity.HasIndex(e => e.DateCreated, "IX_AssetLabels_DateCreated");

            entity.HasIndex(e => new { e.Name, e.NetworkId }, "IX_AssetLabels_Name_NetworkId").IsUnique();

            entity.Property(e => e.Name).HasDefaultValueSql("(N'')");
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.AssetLabels).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AssetTrack>(entity =>
        {
            entity.Property(e => e.Etag).HasColumnName("ETag");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.AssetTrack).HasForeignKey<AssetTrack>(d => d.Id);
        });

        modelBuilder.Entity<AudioInfo>(entity =>
        {
            entity.ToTable("AudioInfo");

            entity.HasIndex(e => e.DateCreated, "IX_AudioInfo_DateCreated");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");
        });

        modelBuilder.Entity<AudioLabel>(entity =>
        {
            entity.HasKey(e => e.Name);

            entity.ToTable("AudioLabel");

            entity.HasIndex(e => e.DateCreated, "IX_AudioLabel_DateCreated");

            entity.HasIndex(e => e.UserId, "IX_AudioLabel_UserId");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");

            entity.HasOne(d => d.User).WithMany(p => p.AudioLabels).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<BusinessInfo>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasDefaultValueSql("(N'')");

            entity.HasOne(d => d.User).WithMany(p => p.BusinessInfos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ChannelGroupType>(entity =>
        {
            entity.HasIndex(e => e.NetworkId, "IX_ChannelGroupTypes_NetworkId");

            entity.HasIndex(e => new { e.UserId, e.NetworkId, e.Type }, "IX_ChannelGroupTypes_UserId_NetworkId_Type").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(100);

            entity.HasOne(d => d.Network).WithMany(p => p.ChannelGroupTypes).HasForeignKey(d => d.NetworkId);

            entity.HasOne(d => d.User).WithMany(p => p.ChannelGroupTypes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ChannelNetwork>(entity =>
        {
            entity.HasIndex(e => e.DateCreated, "IX_ChannelNetworks_DateCreated");

            entity.HasIndex(e => e.UserId, "IX_ChannelNetworks_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");

            entity.HasOne(d => d.User).WithMany(p => p.ChannelNetworks).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<ChannelSync>(entity =>
        {
            entity.ToTable("channelSyncs");

            entity.HasIndex(e => new { e.ChannelId, e.GroupId }, "IX_channelSyncs_ChannelId_GroupId").IsUnique();

            entity.HasIndex(e => e.DateCreated, "IX_channelSyncs_DateCreated");

            entity.HasIndex(e => e.GroupId, "IX_channelSyncs_GroupId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");
            entity.Property(e => e.GroupId).HasMaxLength(100);

            entity.HasOne(d => d.Group).WithMany(p => p.ChannelSyncs).HasForeignKey(d => d.GroupId);
        });

        modelBuilder.Entity<ContactInfo>(entity =>
        {
            entity.HasIndex(e => e.DateCreated, "IX_ContactInfos_DateCreated");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.ContactInfos).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<ContractInfomation>(entity =>
        {
            entity.ToTable("ContractInfomation");

            entity.HasIndex(e => e.DateCreated, "IX_ContractInfomation_DateCreated");

            entity.HasIndex(e => e.OptionId, "IX_ContractInfomation_OptionId");

            entity.HasIndex(e => e.TypeId, "IX_ContractInfomation_TypeId");

            entity.HasIndex(e => e.UserId, "IX_ContractInfomation_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");

            entity.HasOne(d => d.Option).WithMany(p => p.ContractInfomations)
                .HasForeignKey(d => d.OptionId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Type).WithMany(p => p.ContractInfomations)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.ContractInfomations).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<ContractOption>(entity =>
        {
            entity.ToTable("ContractOption");

            entity.HasIndex(e => e.DateCreated, "IX_ContractOption_DateCreated");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");
        });

        modelBuilder.Entity<ContractType>(entity =>
        {
            entity.ToTable("ContractType");

            entity.HasIndex(e => e.DateCreated, "IX_ContractType_DateCreated");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");
        });

        modelBuilder.Entity<DeliveryDetail>(entity =>
        {
            entity.ToTable("DeliveryDetail");

            entity.HasIndex(e => e.AlbumId, "IX_DeliveryDetail_AlbumId");

            entity.HasIndex(e => e.AudioInfoId, "IX_DeliveryDetail_AudioInfoId").IsUnique();

            entity.HasIndex(e => e.DateCreated, "IX_DeliveryDetail_DateCreated");

            entity.HasIndex(e => e.Name, "IX_DeliveryDetail_Name").IsUnique();

            entity.HasIndex(e => e.SourceId, "IX_DeliveryDetail_SourceId").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");

            entity.HasOne(d => d.Album).WithMany(p => p.DeliveryDetails).HasForeignKey(d => d.AlbumId);

            entity.HasOne(d => d.AudioInfo).WithOne(p => p.DeliveryDetail).HasForeignKey<DeliveryDetail>(d => d.AudioInfoId);
        });

        modelBuilder.Entity<DeviceCode>(entity =>
        {
            entity.HasKey(e => e.UserCode);

            entity.Property(e => e.UserCode).HasMaxLength(200);
            entity.Property(e => e.ClientId).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.DeviceCode1)
                .HasMaxLength(200)
                .HasColumnName("DeviceCode");
            entity.Property(e => e.SessionId).HasMaxLength(100);
            entity.Property(e => e.SubjectId).HasMaxLength(200);
        });

        modelBuilder.Entity<FolderDriveMap>(entity =>
        {
            entity.HasKey(e => e.SourceId);

            entity.ToTable("FolderDriveMap");

            entity.HasIndex(e => new { e.Type, e.UserId }, "IX_FolderDriveMap_Type_UserId")
                .IsUnique()
                .HasFilter("([UserId] IS NOT NULL)");

            entity.HasIndex(e => e.UserId, "IX_FolderDriveMap_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.FolderDriveMaps).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<JoinNetwork>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("AuditChanges"));

            entity.HasIndex(e => new { e.ChannelNetworkId, e.NetworkId, e.Type }, "IX_JoinNetworks_ChannelNetworkId_NetworkId_Type").IsUnique();

            entity.HasIndex(e => e.ContractId, "IX_JoinNetworks_ContractId");

            entity.HasIndex(e => e.DateCreated, "IX_JoinNetworks_DateCreated");

            entity.HasIndex(e => e.ManagerId, "IX_JoinNetworks_ManagerId");

            entity.HasIndex(e => e.NetworkId, "IX_JoinNetworks_NetworkId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");

            entity.HasOne(d => d.ChannelNetwork).WithMany(p => p.JoinNetworks)
                .HasForeignKey(d => d.ChannelNetworkId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Contract).WithMany(p => p.JoinNetworks).HasForeignKey(d => d.ContractId);

            entity.HasOne(d => d.Manager).WithMany(p => p.JoinNetworks).HasForeignKey(d => d.ManagerId);

            entity.HasOne(d => d.Network).WithMany(p => p.JoinNetworks).HasForeignKey(d => d.NetworkId);
        });

        modelBuilder.Entity<JoinNetworkHistory>(entity =>
        {
            entity.ToTable("JoinNetworkHistory");

            entity.HasIndex(e => e.DateCreated, "IX_JoinNetworkHistory_DateCreated");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ChannelId).HasMaxLength(100);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");
            entity.Property(e => e.ManagerId).HasMaxLength(256);
            entity.Property(e => e.Status).HasDefaultValueSql("(N'')");
            entity.Property(e => e.UserId).HasMaxLength(256);
        });

        modelBuilder.Entity<MediaAuthor>(entity =>
        {
            entity.HasKey(e => e.Name);

            entity.ToTable("MediaAuthor");

            entity.HasIndex(e => e.DateCreated, "IX_MediaAuthor_DateCreated");

            entity.HasIndex(e => e.UserId, "IX_MediaAuthor_UserId");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");

            entity.HasOne(d => d.User).WithMany(p => p.MediaAuthors).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<MediaChannel>(entity =>
        {
            entity.HasIndex(e => e.DateCreated, "IX_MediaChannels_DateCreated");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.MediaChannels).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<MediaDelivery>(entity =>
        {
            entity.ToTable("MediaDelivery");

            entity.HasIndex(e => e.DateCreated, "IX_MediaDelivery_DateCreated");

            entity.HasIndex(e => e.UserId, "IX_MediaDelivery_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");

            entity.HasOne(d => d.User).WithMany(p => p.MediaDeliveries).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<MediaNetwork>(entity =>
        {
            entity.HasIndex(e => e.DateCreated, "IX_MediaNetworks_DateCreated");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AccountJson).HasDefaultValueSql("(N'')");
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");
            entity.Property(e => e.OwnerId).HasDefaultValueSql("(N'')");
        });

        modelBuilder.Entity<MediaRepositoriesAllowed>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.SourceId });

            entity.ToTable("MediaRepositoriesAllowed");

            entity.HasIndex(e => e.DateCreated, "IX_MediaRepositoriesAllowed_DateCreated");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");

            entity.HasOne(d => d.User).WithMany(p => p.MediaRepositoriesAlloweds)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MediaRepository>(entity =>
        {
            entity.ToTable("MediaRepository");

            entity.HasIndex(e => e.DateCreated, "IX_MediaRepository_DateCreated");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");
        });

        modelBuilder.Entity<MediaRepositoryAsset>(entity =>
        {
            entity.ToTable("MediaRepositoryAsset");

            entity.HasIndex(e => e.DateCreated, "IX_MediaRepositoryAsset_DateCreated");

            entity.HasIndex(e => e.RepositoryId, "IX_MediaRepositoryAsset_RepositoryId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");

            entity.HasOne(d => d.Repository).WithMany(p => p.MediaRepositoryAssets)
                .HasForeignKey(d => d.RepositoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MetaFile>(entity =>
        {
            entity.HasIndex(e => e.DateCreated, "IX_MetaFiles_DateCreated");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");

            entity.HasOne(d => d.MetaFolder).WithMany(p => p.MetaFiles).HasForeignKey(d => d.MetaFolderId);
        });

        modelBuilder.Entity<MetaFolder>(entity =>
        {
            entity.HasIndex(e => e.DateCreated, "IX_MetaFolders_DateCreated");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");
            entity.Property(e => e.DisableDelete)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.DisableEdit)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.IsLock)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.ParentFolder).WithMany(p => p.InverseParentFolder).HasForeignKey(d => d.ParentFolderId);

            entity.HasOne(d => d.User).WithMany(p => p.MetaFolders).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<NamingFolder>(entity =>
        {
            entity.HasKey(e => new { e.FolderId, e.UserId });

            entity.HasIndex(e => e.DateCreated, "IX_NamingFolders_DateCreated");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");

            entity.HasOne(d => d.Folder).WithMany(p => p.NamingFolders).HasForeignKey(d => d.FolderId);

            entity.HasOne(d => d.User).WithMany(p => p.NamingFolders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PaymentInfo>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasDefaultValueSql("(N'')");

            entity.HasOne(d => d.User).WithMany(p => p.PaymentInfos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PersistedGrant>(entity =>
        {
            entity.HasKey(e => e.Key);

            entity.Property(e => e.Key).HasMaxLength(200);
            entity.Property(e => e.ClientId).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.SessionId).HasMaxLength(100);
            entity.Property(e => e.SubjectId).HasMaxLength(200);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasIndex(e => e.DateCreated, "IX_Reports_DateCreated");

            entity.HasIndex(e => e.UserId, "IX_Reports_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");
            entity.Property(e => e.Usdrate).HasColumnName("USDRate");

            entity.HasOne(d => d.User).WithMany(p => p.Reports).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<ReportChannel>(entity =>
        {
            entity.HasKey(e => e.ReportViewId);

            entity.ToTable("ReportChannel");

            entity.Property(e => e.ReportViewId).ValueGeneratedNever();

            entity.HasOne(d => d.ReportView).WithOne(p => p.ReportChannel).HasForeignKey<ReportChannel>(d => d.ReportViewId);
        });

        modelBuilder.Entity<ReportView>(entity =>
        {
            entity.HasIndex(e => e.DateCreated, "IX_ReportViews_DateCreated");

            entity.HasIndex(e => new { e.ReportId, e.ResourceId }, "IX_ReportViews_ReportId_ResourceId")
                .IsUnique()
                .HasFilter("([ResourceId] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportViews).HasForeignKey(d => d.ReportId);
        });

        modelBuilder.Entity<RouteNoteAlbum>(entity =>
        {
            entity.ToTable("RouteNoteAlbum");

            entity.HasIndex(e => e.AuthorName, "IX_RouteNoteAlbum_AuthorName");

            entity.HasIndex(e => e.DateCreated, "IX_RouteNoteAlbum_DateCreated");

            entity.HasIndex(e => e.DeliveryId, "IX_RouteNoteAlbum_DeliveryId");

            entity.HasIndex(e => e.LabelName, "IX_RouteNoteAlbum_LabelName");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");
            entity.Property(e => e.ImageSourceId).HasDefaultValueSql("(N'')");
            entity.Property(e => e.Upc).HasColumnName("UPC");

            entity.HasOne(d => d.AuthorNameNavigation).WithMany(p => p.RouteNoteAlbums).HasForeignKey(d => d.AuthorName);

            entity.HasOne(d => d.Delivery).WithMany(p => p.RouteNoteAlbums)
                .HasForeignKey(d => d.DeliveryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.LabelNameNavigation).WithMany(p => p.RouteNoteAlbums).HasForeignKey(d => d.LabelName);
        });

        modelBuilder.Entity<RouteNoteAudio>(entity =>
        {
            entity.HasKey(e => e.MediaAssetId);

            entity.ToTable("RouteNoteAudio");

            entity.HasIndex(e => e.DateCreated, "IX_RouteNoteAudio_DateCreated");

            entity.Property(e => e.MediaAssetId).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");
            entity.Property(e => e.Isrc).HasColumnName("ISRC");

            entity.HasOne(d => d.MediaAsset).WithOne(p => p.RouteNoteAudio).HasForeignKey<RouteNoteAudio>(d => d.MediaAssetId);
        });

        modelBuilder.Entity<VideoPrice>(entity =>
        {
            entity.HasIndex(e => e.DateCreated, "IX_VideoPrices_DateCreated");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.VideoPrices).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Whitelist>(entity =>
        {
            entity.HasIndex(e => new { e.ChannelId, e.NetworkId }, "IX_Whitelists_ChannelId_NetworkId")
                .IsUnique()
                .HasFilter("([ChannelId] IS NOT NULL)");

            entity.HasIndex(e => e.DateCreated, "IX_Whitelists_DateCreated");

            entity.HasIndex(e => e.NetworkId, "IX_Whitelists_NetworkId");

            entity.HasIndex(e => e.UserId, "IX_Whitelists_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateUpdated).HasDefaultValueSql("('1970-01-01T00:00:00.0000000')");

            entity.HasOne(d => d.Network).WithMany(p => p.Whitelists)
                .HasForeignKey(d => d.NetworkId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.Whitelists).HasForeignKey(d => d.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
