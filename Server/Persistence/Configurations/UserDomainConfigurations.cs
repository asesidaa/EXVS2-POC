using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Server.Models.Cards;

namespace Server.Persistence.Configurations;

public class UserDomainConfigurations : IEntityTypeConfiguration<UserDomain>
{
    public void Configure(EntityTypeBuilder<UserDomain> builder)
    {
        builder.HasKey(x => x.UserId);
    }
}