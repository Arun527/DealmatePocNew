namespace DealmateApi.Service.Pagination;

public static class PaginationExtensions
{
    public static IQueryable<T> ApplyPagination<T, TFilter>(IQueryable<T> query, TFilter filter)
  where T : class
    {
        int skip = 0;
        int take = 50;
        var pagination = filter!.GetType().GetProperty("Pagination")?.GetValue(filter) as Pagination;
        if (pagination != null)
        {
            // Set the 'take' value to the MaxResults if specified, otherwise use the default
            if (pagination.MaxResults.HasValue && pagination.MaxResults.Value > 0)
            {
                take = pagination.MaxResults.Value;
            }

            // Calculate the number of records to skip based on the current page
            if (pagination.Page.HasValue && pagination.Page.Value > 1)
            {
                skip = (pagination.Page.Value - 1) * take;
            }
        }

        return query.Skip(skip).Take(take);
    }
}
