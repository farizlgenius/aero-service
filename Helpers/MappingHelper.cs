using System.Linq.Expressions;

namespace HIDAeroService.Helpers
{
    public static class MappingHelper
    {
        private static readonly Dictionary<string, Delegate> _cache = new();

        public static TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : new()
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var key = $"{typeof(TSource).FullName}->{typeof(TDestination).FullName}";
            if (!_cache.TryGetValue(key, out var mapFunc))
            {
                var sourceParam = Expression.Parameter(typeof(TSource), "src");

                var bindings = typeof(TDestination)
                    .GetProperties()
                    .Where(destProp => destProp.CanWrite)
                    .Select(destProp =>
                    {

                        var sourceProp = typeof(TSource).GetProperty(destProp.Name);
                        if (sourceProp == null || sourceProp.PropertyType != destProp.PropertyType)
                            return null;

                        var sourceValue = Expression.Property(sourceParam, sourceProp);
                        return Expression.Bind(destProp, sourceValue);
                    })
                    .Where(b => b != null)
                    .ToList();

                var body = Expression.MemberInit(Expression.New(typeof(TDestination)), bindings);
                var lambda = Expression.Lambda<Func<TSource, TDestination>>(body, sourceParam);
                mapFunc = lambda.Compile();
                _cache[key] = mapFunc;
            }

            return ((Func<TSource, TDestination>)mapFunc)(source);
        }

        public static List<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> sources)
            where TDestination : new()
        {
            return sources.Select(Map<TSource, TDestination>).ToList();
        }
    }
}
