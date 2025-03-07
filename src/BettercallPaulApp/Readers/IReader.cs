namespace BettercallPaulApp.Readers;

public interface IReader
{
   IEnumerable<string[]> ReadData(string filePath, char delimiter = ',');
}