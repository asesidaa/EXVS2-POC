using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebUIOver.Shared.Models;

namespace ServerOver.Common.Utils;

public static class PaginateUtls
{
    public static async Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable,
        int pageNumber,
        int pageSize,
        Expression<Func<TDestination, int>> selectorFunction,
        CancellationToken cancellationToken)
    {
        var count = await queryable.CountAsync(cancellationToken);

        var items = await queryable
            .OrderBy(selectorFunction)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<TDestination>(items, count, pageNumber, pageSize);
    }
}
