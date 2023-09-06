using System.ComponentModel;

namespace Server.Common.Enum;

public enum DamageLevel : uint
{
    [Description("1")]
    One = 1,
    [Description("2")]
    Two = 2,
    [Description("3")]
    Three = 3,
    [Description("4")]
    Four = 4,
    [Description("5")]
    Five = 5,
    [Description("6")]
    Six = 6,
    [Description("7")]
    Seven = 7,
    [Description("8")]
    Eight = 8
}