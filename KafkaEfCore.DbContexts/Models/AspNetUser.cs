using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class AspNetUser
{
    public string Id { get; set; } = null!;

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? DisplayName { get; set; }

    public string? Avatar { get; set; }

    public double DiscountRate { get; set; }

    public string? ForderShareId { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DatePaymentRequest { get; set; }

    public string? DriveFolderId { get; set; }

    public string? DriveDeleteId { get; set; }

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public string? AutoPassword { get; set; }

    public string? UserCreatorId { get; set; }

    public string? PersonAddress { get; set; }

    public string? PersonIdentityCard { get; set; }

    public DateTime SigningDate { get; set; }

    public string? Tags { get; set; }

    public string? TokenStamp { get; set; }

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    public virtual ICollection<AssetLabel> AssetLabels { get; set; } = new List<AssetLabel>();

    public virtual ICollection<AudioLabel> AudioLabels { get; set; } = new List<AudioLabel>();

    public virtual ICollection<BusinessInfo> BusinessInfos { get; set; } = new List<BusinessInfo>();

    public virtual ICollection<ChannelGroupType> ChannelGroupTypes { get; set; } = new List<ChannelGroupType>();

    public virtual ICollection<ChannelNetwork> ChannelNetworks { get; set; } = new List<ChannelNetwork>();

    public virtual ICollection<ContactInfo> ContactInfos { get; set; } = new List<ContactInfo>();

    public virtual ICollection<ContractInfomation> ContractInfomations { get; set; } = new List<ContractInfomation>();

    public virtual ICollection<FolderDriveMap> FolderDriveMaps { get; set; } = new List<FolderDriveMap>();


    public virtual ICollection<JoinNetwork> JoinNetworks { get; set; } = new List<JoinNetwork>();

    public virtual ICollection<MediaAuthor> MediaAuthors { get; set; } = new List<MediaAuthor>();

    public virtual ICollection<MediaChannel> MediaChannels { get; set; } = new List<MediaChannel>();

    public virtual ICollection<MediaDelivery> MediaDeliveries { get; set; } = new List<MediaDelivery>();

    public virtual ICollection<MediaRepositoriesAllowed> MediaRepositoriesAlloweds { get; set; } = new List<MediaRepositoriesAllowed>();

    public virtual ICollection<MetaFolder> MetaFolders { get; set; } = new List<MetaFolder>();

    public virtual ICollection<NamingFolder> NamingFolders { get; set; } = new List<NamingFolder>();

    public virtual ICollection<PaymentInfo> PaymentInfos { get; set; } = new List<PaymentInfo>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual ICollection<VideoPrice> VideoPrices { get; set; } = new List<VideoPrice>();

    public virtual ICollection<Whitelist> Whitelists { get; set; } = new List<Whitelist>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}
