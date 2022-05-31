using Pacman.Business.Model;
using Pacman.Business.View;
using Pacman.Exceptions;

namespace Pacman.Business.Control;

public class GameService
{
    private readonly IReader _reader;
    private readonly IWriter _writer;

    public GameService(IReader reader, IWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public GameState GetNewGameState(string filepath)
    {
        var size = _getFileSize(filepath);
        var pac = new Pac(new Coordinate(), _reader, _writer);
        var walls = Array.Empty<Wall>();
        var ghosts = Array.Empty<MovableEntity>();
        var pellets = new Dictionary<Coordinate, Pellet>();

        return new GameState(size, pac, walls, pellets, ghosts);
    }

    private static Size _getFileSize(string filepath)
    {
        var fileLines = File.ReadAllLines(filepath);
        var length = fileLines.Length;
        var width = fileLines.First().Length;

        if (fileLines.Any(line => line.Length != width))
            throw new InvalidFileException();

        return new Size(width, length);
    }
}