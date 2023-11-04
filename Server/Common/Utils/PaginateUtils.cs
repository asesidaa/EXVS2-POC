using Microsoft.EntityFrameworkCore;

namespace Server.Common.Utils;

public static class PaginateUtls
{
    public static async Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var count = await queryable.CountAsync(cancellationToken);

        var items = await queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PaginatedList<TDestination>(items, count, pageNumber, pageSize);
    }
}
