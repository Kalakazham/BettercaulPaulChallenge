namespace BettercallPaulApp.Readers;

public class CsvReader : IReader
{
    public IEnumerable<string[]> ReadData(string filePath, char delimiter = ',')
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The file at '{filePath}' was not found.");
        }

        try
        {
            var lines = File.ReadAllLines(filePath);

            if (lines.Length <= 1)
            {
                throw new InvalidOperationException($"The file '{filePath}' contains no data.");
            }

            return lines.Skip(1)
                .Select(line => line.Split(delimiter))
                .ToList();
        }
        catch (IOException ex)
        {
            throw new IOException($"Error reading the file '{filePath}': {ex.Message}", ex);
        }
    }
}