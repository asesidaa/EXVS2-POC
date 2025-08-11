using Riok.Mapperly.Abstractions;
using ServerOver.Views.Battle;
using WebUIOver.Shared.Dto.Rank;

namespace ServerOver.Mapper.Rank;

[Mapper]
public static partial class PlayerLevelRankMapper
{
    public static partial PlayerLevelRankDto ToPlayerLevelRankDto(this PlayerLevelRankView playerLevelRankView);
}