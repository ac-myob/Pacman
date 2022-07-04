using System.Collections.Generic;
using System.Linq;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.Selector;
using Pacman.Business.Control.WorldLoader;
using Pacman.Business.Model;
using Pacman.Business.Model.Ghosts;
using Pacman.Variables;
using Xunit;

namespace Pacman.Tests;

public class GameEngineTests
{
    private readonly GameEngine _gameEngine;
    private readonly Mock<ISelector<Coordinate>> _mockSelector = new();

    public GameEngineTests()
    {
        var ghostFactory = new GhostFactory(_mockSelector.Object);
        var worldBuilder = new WorldBuilder(new FileLoader("../../../../Pacman/Games/TestMap.txt"), ghostFactory);
        _gameEngine = new GameEngine(worldBuilder.GetEntities(), ghostFactory, _mockSelector.Object);
    }
    
    
    [Fact]
    public void PlayRound_MovesPac_WhenGivenDirection()
    {
        _gameEngine.PlayRound(Direction.North);
        
        Assert.Equal(new Coordinate(2, 0), _gameEngine.GameState.Pac.Coordinate);
    }
    
    [Fact]
    public void PlayRound_MovesGhosts()
    {
        var expectedGhostCoords = new Coordinate[] {new(0, 1), new(0, 2), new(1, 2)};
        _mockSelector.Setup(_ => _.SelectFrom(It.IsAny<IEnumerable<Coordinate>>())).
            Returns(new Coordinate(0, 2));

        _gameEngine.PlayRound(Direction.West);
        var actualGhostCoords = _gameEngine.GameState.Ghosts.Select(g => g.Coordinate);
        
        Assert.Equal(expectedGhostCoords, actualGhostCoords);
    }
    
    [Fact]
    public void PlayRound_RemovesPelletTraversedByPac_WhenNoGhostOnPellet()
    {
        _mockSelector.Setup(_ => _.SelectFrom(It.IsAny<IEnumerable<Coordinate>>())).
            Returns(new Coordinate(0, 2));
        
        _gameEngine.PlayRound(Direction.West);
        var pelletCoords = _gameEngine.GameState.GetPellets().Select(p => p.Coordinate); 
        
        Assert.DoesNotContain(new Coordinate(1, 1), pelletCoords);
    }
    
    [Fact]
    public void PlayRound_DoesNotRemovePelletTraversedByPac_WhenGhostOnPellet()
    {
        _mockSelector.Setup(_ => _.SelectFrom(It.IsAny<IEnumerable<Coordinate>>())).
            Returns(new Coordinate(1, 1));
        
        _gameEngine.PlayRound(Direction.West);
        var pelletCoords = _gameEngine.GameState.GetPellets().Select(p => p.Coordinate); 
        
        Assert.Contains(new Coordinate(1, 1), pelletCoords);
    }
    
    [Fact]
    public void PlayRound_GivesPacPowerUp_WhenPacTraversesPowerPellet()
    {
        _mockSelector.Setup(_ => _.SelectFrom(It.IsAny<IEnumerable<Coordinate>>())).
            Returns(new Coordinate(0, 2));
        
        _gameEngine.PlayRound(Direction.North);
        
        Assert.Equal(Constants.PowerUpTurns, _gameEngine.GameState.Pac.PowerUp);
    }
    
    [Fact]
    public void PlayRound_ReducesPacPowerUp_WhenPowerUpGreaterThanZero()
    {
        _mockSelector.Setup(_ => _.SelectFrom(It.IsAny<IEnumerable<Coordinate>>())).
            Returns(new Coordinate(0, 2));
        _gameEngine.GameState.Pac.AddPowerUp();
        
        _gameEngine.PlayRound(Direction.West);
        
        Assert.Equal(Constants.PowerUpTurns - 1, _gameEngine.GameState.Pac.PowerUp);
    }
    
    [Fact]
    public void PlayRound_UpdatesGameStatusToCollided_WhenPacOnGhost()
    {
        _mockSelector.Setup(_ => _.SelectFrom(It.IsAny<IEnumerable<Coordinate>>())).
            Returns(new Coordinate(1, 1));

        _gameEngine.PlayRound(Direction.West);

        Assert.Equal(GameStatus.Collided, _gameEngine.GameState.GameStatus);
    }
    
    [Fact]
    public void PlayRound_UpdatesGameStatusToRoundComplete_WhenNoPelletsRemaining()
    {
        _gameEngine.PlayRound(Direction.North);
        _gameEngine.PlayRound(Direction.West);
        _gameEngine.PlayRound(Direction.West);
        _gameEngine.PlayRound(Direction.South);
        _gameEngine.PlayRound(Direction.East);

        Assert.Equal(GameStatus.RoundComplete, _gameEngine.GameState.GameStatus);
    }
    
    [Fact]
    public void ResetRound_ResetsGhostAndPacCoordinates()
    {
        var expectedCoords = new Coordinate[] {new(2, 1), new(0, 2), new(1, 2), new(1, 3)};
        _mockSelector.Setup(_ => _.SelectFrom(It.IsAny<IEnumerable<Coordinate>>())).
            Returns(new Coordinate(2, 2));

        _gameEngine.PlayRound(Direction.West);
        _gameEngine.ResetRound();
        var actualCoords = _gameEngine.GameState.Ghosts.Select(g => g.Coordinate).
            Prepend(_gameEngine.GameState.Pac.Coordinate);

        Assert.Equal(expectedCoords, actualCoords);
    }
    
    [Fact]
    public void ResetRound_ReducesPacLifeByOne_WhenLivesGreaterThanZero()
    {
        var pacLives = _gameEngine.GameState.Pac.Lives;
        
        _gameEngine.ResetRound();

        Assert.Equal(pacLives - 1, _gameEngine.GameState.Pac.Lives);
    }
    
    [Fact]
    public void ResetRound_SetsGameStatusToRunning_WhenPacLivesGreaterThanZero()
    {
        _gameEngine.ResetRound();

        Assert.Equal(GameStatus.Running, _gameEngine.GameState.GameStatus);
    }
    
    [Fact]
    public void ResetRound_SetsGameStatusToGameComplete_WhenPacLivesEqualsZero()
    {
        while (_gameEngine.GameState.Pac.Lives > 0)
            _gameEngine.GameState.Pac.ReduceLife();
        
        _gameEngine.ResetRound();

        Assert.Equal(GameStatus.GameComplete, _gameEngine.GameState.GameStatus);
    }
}
