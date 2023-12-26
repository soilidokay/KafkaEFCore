using System;
using System.Collections.Generic;

namespace KafkaEfCore.DbContexts.Models;

public partial class AudioInfo
{
    public Guid Id { get; set; }

    public double Bitrate { get; set; }

    public double SampleRate { get; set; }

    public double BitsPerSample { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public virtual DeliveryDetail? DeliveryDetail { get; set; }
}
