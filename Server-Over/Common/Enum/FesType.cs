using System.ComponentModel;

namespace ServerOver.Common.Enum;

public enum FesType : uint
{
    [Description("DualSelect")]
    DualSelect = 1,
    [Description("ScoreBattle")]
    ScoreBattle = 2,
    [Description("AttackScoreBattle")]
    AttackScoreBattle = 4,
    [Description("AttackBoost")]
    AttackBoost = 5,
    [Description("UnlimitedExBurst")]
    UnlimitedExBurst = 6,
    [Description("ExMugenShooting")]
    ExMugenShooting = 7,
    [Description("OneOnOneAllCost")]
    OneOnOneAllCost = 10,
    [Description("OneOnOne3000")]
    OneOnOne3000 = 11,
    [Description("OneOnOne2500")]
    OneOnOne2500 = 12,
    [Description("OneOnOne2000")]
    OneOnOne2000 = 13,
    [Description("OneOnOne1500")]
    OneOnOne1500 = 14,
    [Description("OneOnOneUnlimitedExBurstAllCost")]
    OneOnOneUnlimitedExBurstAllCost = 20,
    [Description("OneOnOneUnlimitedExBurst3000")]
    OneOnOneUnlimitedExBurst3000 = 21,
    [Description("OneOnOneUnlimitedExBurst2500")]
    OneOnOneUnlimitedExBurst2500 = 22,
    [Description("OneOnOneUnlimitedExBurst2000")]
    OneOnOneUnlimitedExBurst2000 = 23,
    [Description("OneOnOneUnlimitedExBurst1500")]
    OneOnOneUnlimitedExBurst1500 = 24
}