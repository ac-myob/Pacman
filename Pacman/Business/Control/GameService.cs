using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Pacman.Business.View;
using Pacman.Exceptions;
using Pacman.Variables;

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
        var fileLines = File.ReadAllLines(filepath);
        if (!fileLines.Any())
            throw new InvalidFileException("File is empty.");
        
        var length = fileLines.Length;
        var width = fileLines.First().Length;

        if (fileLines.Any(line => line.Length != width))
            throw new InvalidFileException("File width must be uniform.");
        
        var size = new Size(width, length);
        var pac = new Pac(_getPacCoord(fileLines, size), _reader, _writer);
        var walls = new List<Wall>();
        var ghosts = new List<MovableEntity>();
        var pellets = new List<Pellet>();
        
        for (var l = 0; l < length; l++)
            for (var w = 0; w < width; w++)
            {
                var currentCoord = new Coordinate(w, l);
                switch (fileLines[l][w])
                {
                    case Constants.Wall:
                        walls.Add(new Wall(currentCoord));
                        break;
                    case Constants.RandomGhost:
                        ghosts.Add(new RandomGhost(currentCoord, new RandomSelector<Coordinate>()));
                        break;
                    case Constants.GreedyGhost:
                        ghosts.Add(new GreedyGhost(currentCoord, pac));
                        break;
                    case Constants.PathFindingGhost:
                        ghosts.Add(new PathFindingGhost(currentCoord, pac));
                        break;
                    case Constants.Pellet:
                        pellets.Add(new Pellet(currentCoord));
                        break;
                }
            }

        return new GameState(size, pac, walls, pellets, ghosts);
    }

    private static Coordinate _getPacCoord(IReadOnlyList<string> fileLines, Size size)
    {
        var pacCoord = new Coordinate();
        var pacCount = 0;
        var (width, length) = size;

        for (var l = 0; l < length; l++)
            for (var w = 0; w < width; w++)
            {
                if (fileLines[l][w] != Constants.PacStart) continue;
                    
                pacCoord = new Coordinate(w, l);
                ++pacCount;
            }

        return pacCount switch
        {
            0 => throw new InvalidFileException("Pacman symbol not found in file."),
            > 1 => throw new InvalidFileException("More than one Pacman symbol was found in file."),
            _ => pacCoord
        };
    }

    public static void ResetGameState(GameState gameState)
    {
        foreach (var movableEntity in gameState.MovableEntities)
            movableEntity.ResetCoordinate();
    }
}
