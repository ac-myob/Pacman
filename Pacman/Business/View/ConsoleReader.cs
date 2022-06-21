namespace Pacman.Business.View;

public class ConsoleReader : IReader
{
    public string ReadKey()
    {
        return Console.ReadKey().Key.ToString();
    }
}
