﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Replay;

namespace ServerOver.Persistence.Configurations.Cards.Replay;

public class SharedUploadReplayConfigurations : IEntityTypeConfiguration<SharedUploadReplay>
{
    public void Configure(EntityTypeBuilder<SharedUploadReplay> builder)
    {
        builder.HasKey(x => x.ReplayId);
    }
}