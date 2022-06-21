using Pacman.Variables;

namespace Pacman.Business.View;

public class ConsoleWriter : IWriter
{
    public void Write(string message) => Console.Write(message);

    public void Write(string message, IDictionary<char, Colour> colourMapping)
    {
        foreach (var character in message)
        {
            if (colourMapping.ContainsKey(character))
                Console.ForegroundColor = colourMapping[character] switch
                {
                    Colour.Red => ConsoleColor.Red,
                    Colour.Blue => ConsoleColor.Blue,
                    Colour.Yellow => ConsoleColor.Yellow,
                    Colour.Green => ConsoleColor.Green,
                    Colour.Cyan => ConsoleColor.Cyan,
                    _ => ConsoleColor.Black
                };

            Console.Write(character);
            Console.ResetColor();
        }
    }

    public void Clear() => Console.Clear();
}
