using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerVanilla.Models.Cards.MobileSuit;

namespace ServerVanilla.Persistence.Configurations.Cards.MobileSuit;

public class FavouriteMobileSuitConfigurations : IEntityTypeConfiguration<FavouriteMobileSuit>
{
    public void Configure(EntityTypeBuilder<FavouriteMobileSuit> builder)
    {
        builder.HasKey(x => x.Id);
    }
}