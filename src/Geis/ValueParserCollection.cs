namespace Geis;

public class ValueParserCollection
{
    private readonly Dictionary<Type, Func<string, object?>> _parsers = new();

    public ValueParserCollection SetParser<T>(Func<string, T?> parser) where T : notnull
    {
        _parsers[typeof(T)] = s => parser(s);
        return this;
    }
}

public static class ValueParserCollectionExtensions
{
    public static ValueParserCollection AddDefaultParser<T>(this ValueParserCollection collection) => collection;
}
