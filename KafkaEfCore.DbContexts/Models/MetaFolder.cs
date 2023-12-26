using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class MetaFolder
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? DriveFolderId { get; set; }

    public string? UserId { get; set; }

    public int Adjacency { get; set; }

    public string? Trace { get; set; }

    public Guid? ParentFolderId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public bool? IsLock { get; set; }

    public bool? DisableDelete { get; set; }

    public bool? DisableEdit { get; set; }

    public virtual ICollection<MetaFolder> InverseParentFolder { get; set; } = new List<MetaFolder>();

    public virtual ICollection<MetaFile> MetaFiles { get; set; } = new List<MetaFile>();

    public virtual ICollection<NamingFolder> NamingFolders { get; set; } = new List<NamingFolder>();

    public virtual MetaFolder? ParentFolder { get; set; }

    public virtual AspNetUser? User { get; set; }
}
