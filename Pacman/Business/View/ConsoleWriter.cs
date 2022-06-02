using Pacman.Variables;

namespace Pacman.Business.View;

public class ConsoleWriter : IWriter
{
    public void Write(string message)
    {
        Console.Write(message);
    }

    public void Clear()
    {
        Console.Clear();
    }
}