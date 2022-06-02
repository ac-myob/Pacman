using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Pacman.Business.View;
using Pacman.Variables;

namespace Pacman.Business.Control;

public class GameEngine
{
    private readonly IWriter _writer;
    private readonly IReader _reader;
    private readonly GameService _gameService;
    private GameState _gameState = null!;
    private readonly IDictionary<char, Colour> _colourMapping = new Dictionary<char, Colour>
    {
        {Constants.Heart, Colour.Red},
        {Constants.RandomGhost, Colour.Blue},
        {Constants.GreedyGhost, Colour.Green},
        {Constants.PathFindingGhost, Colour.Red},
        {Constants.PacStart, Colour.Yellow},
        {Constants.PacUp, Colour.Yellow},
        {Constants.PacDown, Colour.Yellow},
        {Constants.PacLeft, Colour.Yellow},
        {Constants.PacRight, Colour.Yellow}
    };
    
    public GameEngine(IReader reader, IWriter writer)
    {
        _reader = reader;
        _writer = writer;
        _gameService = new GameService(reader, writer);
    }
    
    public void Run()
    {
        _gameState = _gameService.GetNewGameState(Constants.GameFilepath);
        
        while (!_gameState.FinishedGame() && _gameState.Pac.Lives > 0)
        {
            
            foreach (var movableEntity in _gameState.MovableEntities)
            {
                _writer.Clear();
                _writer.Write(Messages.GetTurnInfo(_gameState) + _gameState.GetString(), _colourMapping);
                movableEntity.Move(_gameState);
            }
            
            if (_gameState.IsPacOnGhost())
            {
                _writer.Write(Messages.GhostCollision);
                _reader.ReadKey();
                _gameState.ResetMovableEntities();
                _gameState.Pac.ReduceLife();
            }

            if (!_gameState.Pellets.Any())
            {
                _gameState.ResetMovableEntities();
                _gameState.ResetPellets();
                _gameState.IncreaseRound();
                _gameState.AddGhost(new RandomSelector<Coordinate>());
            }
        }
        _writer.Write(Messages.GetTurnInfo(_gameState) + _gameState.GetString(), _colourMapping);
        _writer.Write(Messages.GetGameOutcome(_gameState));
    }
}