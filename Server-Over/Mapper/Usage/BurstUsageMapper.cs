using Riok.Mapperly.Abstractions;
using ServerOver.Views.Usage;
using WebUIOver.Shared.Dto.Usage;

namespace ServerOver.Mapper.Usage;

[Mapper]
public static partial class BurstUsageMapper
{
    public static partial BurstUsageDto ToBurstUsageDto(this BurstUsageView burstUsageView);
}