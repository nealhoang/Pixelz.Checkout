namespace Pixelz.Application.Common.Mapping;

public class PagedResultConverter<TSource, TDestination> : ITypeConverter<PagedResult<TSource>, PagedResult<TDestination>>
{
    public PagedResult<TDestination> Convert(
        PagedResult<TSource> source,
        PagedResult<TDestination> destination,
        ResolutionContext context)
    {
        return new PagedResult<TDestination>
        {
            PageIndex = source.PageIndex,
            PageSize = source.PageSize,
            TotalCount = source.TotalCount,
            Items = context.Mapper.Map<List<TDestination>>(source.Items)
        };
    }
}
