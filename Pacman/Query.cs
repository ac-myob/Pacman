using System.Text.RegularExpressions;

namespace Pacman;

public class Query
{
    private readonly IReader _reader;
    private readonly IWriter _writer;

    public Query(IReader reader, IWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public string GetKeyPress(string validKeysRegex, string? invalidMessage = null)
    {
        invalidMessage ??= $"Key press does not match the regex {validKeysRegex}. Please try again: ";
        string keyPress;
        bool validKeyPress;

        do
        {
            keyPress = _reader.ReadKey();
            validKeyPress = Regex.IsMatch(keyPress, validKeysRegex);
            
            if (!validKeyPress) _writer.WriteLine(invalidMessage);

        } while (!validKeyPress);

        return keyPress;
    }
}
