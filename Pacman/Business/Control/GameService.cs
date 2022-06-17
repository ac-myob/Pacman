using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public class GameService : IGameService
{
    private readonly GameEngine _gameEngine;
    public GameState GameState => _gameEngine.GameState;

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
}
