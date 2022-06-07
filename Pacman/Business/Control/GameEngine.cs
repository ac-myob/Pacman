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
        _displayMap();
        
        do
        {
            foreach (var movableEntity in _gameState.GetMovableEntities())
                _gameState = movableEntity.Move(_gameState);

            _gameState = _gameState.UpdatePellets();
            _displayMap();

            if (_gameState.IsPacOnGhost())
            {
                _writer.Write(Messages.GhostCollision);
                _reader.ReadKey();
                _gameState = _gameService.GetResetGameState(_gameState);
                _displayMap();
            }
            else if (!_gameState.GetPellets().Any())
            {
                _writer.Write(Messages.RoundComplete);
                _reader.ReadKey();
                _gameState = _gameService.GetNextRoundGameState(_gameState);
                _displayMap();
            }

        } while (!_gameState.IsGameFinished() && _gameState.Lives > 0);
        
        _writer.Write(Messages.GetGameOutcome(_gameState));
    }
    
    private void _displayMap() {
        _writer.Clear();
        _writer.Write(Messages.GetTurnInfo(_gameState) + _gameState.GetString(), _colourMapping);
    }
}