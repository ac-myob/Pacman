namespace Pacman.Exceptions;

public class InvalidFileException : Exception
{
    public InvalidFileException(string message) : base(message) { }
}
