using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.Selector;
using Pacman.Business.Control.WorldLoader;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public class GameService : IGameService
{
    private readonly GameEngine _gameEngine;
    private GameState GameState => _gameEngine.GameState;

    public GameService()
    {
        var randomSelector = new RandomSelector<Coordinate>();
        var ghostFactory = new GhostFactory(randomSelector);
        var worldBuilder = new WorldBuilder(new FileLoader(Constants.GameFilepath), ghostFactory);
        _gameEngine = new GameEngine(worldBuilder.GetEntities(), ghostFactory, randomSelector);
    }
    
    public void PlayRound(Direction inputDirection) => _gameEngine.PlayRound(inputDirection);
    public void ResetRound() => _gameEngine.ResetRound();
    public void IncreaseRound() => _gameEngine.IncreaseRound();
    public bool IsGameFinished() => GameState.GameStatus == GameStatus.GameComplete;

    public bool IsRoundComplete() => GameState.GameStatus == GameStatus.RoundComplete;

    public bool IsGameRunning() => GameState.GameStatus == GameStatus.Running;

    public bool IsPacOnGhost() => GameState.GameStatus == GameStatus.Collided;

    public bool IsDirectionValid(Direction inputDirection)
    {
        var currentPacCoord = GameState.Pac.Coordinate;
        var newPacCoord = currentPacCoord.Shift(inputDirection, GameState.Size);

        return !GameState.Walls.ContainsKey(newPacCoord);
    }

    public string GameMap() => Messages.GetTurnInfo(GameState) + GameState.GetString();
}
