using System.ComponentModel;

namespace ServerOver.Dtos.Request;

public class GetAllBareboneCardRequest
{
    [DefaultValue(1)]
    public int Page { get; init; } = 1;

    [DefaultValue(10)]
    public int PageSize { get; init; } = 10;

    [Description("Like search on userName and chipId fields.")]
    public string? SearchKeyword { get; init; }
}