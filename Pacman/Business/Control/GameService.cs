using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.MoveStrategies;
using Pacman.Business.Control.Selector;
using Pacman.Business.Control.Sequence;
using Pacman.Business.Model;
using Pacman.Business.View;
using Pacman.Exceptions;
using Pacman.Variables;

namespace Pacman.Business.Control;

public class GameService
{
    private readonly IReader _reader;
    private readonly IWriter _writer;
    private readonly ISelector<Coordinate> _selector = new RandomSelector<Coordinate>();
    private readonly NumberSequence _numberSequence = new(Constants.StartingId);
    private readonly GhostTypeSequence _ghostTypeSequence = new();
    private readonly GhostFactory _ghostFactory;
    private GameState? _initialGameState;

    public GameService(IReader reader, IWriter writer)
    {
        _reader = reader;
        _writer = writer;
        _ghostFactory = new GhostFactory(_selector);
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
        var pac = new Pac(
            _getPacCoord(fileLines, size), 
            Constants.PacStart,
            new PacMoveStrategy(_reader, _writer));
        var walls = new List<Wall>();
        var ghosts = new List<Ghost>();
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
                        ghosts.Add(_ghostFactory.GetGhost(GhostType.Random, currentCoord, _numberSequence.GetNext()));
                        break;
                    case Constants.GreedyGhost:
                        ghosts.Add(_ghostFactory.GetGhost(GhostType.Greedy, currentCoord, _numberSequence.GetNext()));
                        break;
                    case Constants.PathFindingGhost:
                        ghosts.Add(_ghostFactory.GetGhost(GhostType.PathFinding, currentCoord, _numberSequence.GetNext()));
                        break;
                    case Constants.Pellet:
                        pellets.Add(new Pellet(currentCoord, Constants.Pellet));
                        break;
                    case Constants.MagicPellet:
                        pellets.Add(new Pellet(currentCoord, Constants.MagicPellet));
                        break;
                }
            }
        
        return _initialGameState = new GameState(size, Constants.PacStartingLives, 0, Constants.StartRound, pac, ghosts, walls, pellets);
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

    public GameState GetNextRoundGameState(GameState gameState)
    {
        if (_initialGameState is null)
            throw new InvalidOperationException();

        _initialGameState = _initialGameState with
        {   
            Lives = gameState.Lives,
            Round = gameState.Round + 1,
            Ghosts = _getNewGhosts(),
        };

        return _initialGameState;
    }

    private IEnumerable<Ghost> _getNewGhosts()
    {
        var newGhostType = _ghostTypeSequence.GetNext();
        var posNewGhostCoords = _initialGameState!.Pellets.Select(p => p.Coordinate).
            Except(_initialGameState.Ghosts.Select(g => g.Coordinate)).ToArray();

        // If no space to add ghost, don't add ghost
        if (!posNewGhostCoords.Any()) return _initialGameState.Ghosts;
        
        var newGhostCoord = _selector.SelectFrom(posNewGhostCoords);
        var newGhost = _ghostFactory.GetGhost(newGhostType, newGhostCoord, _numberSequence.GetNext());

        return _initialGameState.Ghosts.Append(newGhost);
    }

    public GameState GetResetGameState(GameState gameState)
    {
        if (_initialGameState is null)
            throw new InvalidOperationException();

        return _initialGameState with
        {
            Lives = gameState.Lives - 1,
            Pellets = gameState.Pellets
        };
    }
}
