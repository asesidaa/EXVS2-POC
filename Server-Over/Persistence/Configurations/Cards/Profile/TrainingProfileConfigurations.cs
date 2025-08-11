using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Profile;

namespace ServerOver.Persistence.Configurations.Cards.Profile;

public class TrainingProfileConfigurations : IEntityTypeConfiguration<TrainingProfile>
{
    public void Configure(EntityTypeBuilder<TrainingProfile> builder)
    {
        builder.HasKey(x => x.Id);
    }
}