using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class NamingFolder
{
    public Guid FolderId { get; set; }

    public string UserId { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public virtual MetaFolder Folder { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
