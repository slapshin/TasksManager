namespace Web.Common.Mapper
{
    public interface IMapper
    {
        TDest Map<TSource, TDest>(TSource source);

        TDest Map<TSource, TDest>(TSource source, TDest dest);
    }
}