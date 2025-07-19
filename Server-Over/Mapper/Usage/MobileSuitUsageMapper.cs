using Riok.Mapperly.Abstractions;
using ServerOver.Views.Usage;
using WebUIOver.Shared.Dto.Usage;

namespace ServerOver.Mapper.Usage;

[Mapper]
public static partial class MobileSuitUsageMapper
{
    public static partial MobileSuitUsageDto ToMobileSuitUsageDto(this MobileSuitUsageView mobileSuitUsageView);
}