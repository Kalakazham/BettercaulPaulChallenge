namespace BettercallPaulApp.Parsers;

public interface IParser<out T>
{
    public IEnumerable<T> Parse(IEnumerable<string[]> csvData);
}