using System;
using System.Linq;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Pacman.Business.View;
using Pacman.Exceptions;
using Xunit;

namespace Pacman.Tests;

public class GameServiceTests
{
    private readonly Mock<IReader> _mockReader = new();
    private readonly Mock<IWriter> _mockWriter = new();
    private readonly GameService _gameService;
    private const string TestGame = "../../../../Pacman/Games/TestGame.txt";

    public GameServiceTests()
    {
        _gameService = new GameService(_mockReader.Object, _mockWriter.Object);
    }

    [Fact]
    public void GetNewGameState_ReturnsGameStateWithCorrectSize_WhenGivenTxtFile()
    {
        var actualGameState = _gameService.GetNewGameState(TestGame);

        Assert.Equal(new Size(3, 4), actualGameState.Size);
    }

    [Fact]
    public void GetNewGameState_ThrowsInvalidFileException_WhenFileWidthIsNotUniform()
    {
        Assert.Throws<InvalidFileException>(
            () => _gameService.GetNewGameState("../../../../Pacman/Games/JaggedFile.txt"));
    }

    [Fact]
    public void GetNewGameState_ThrowsInvalidFileException_WhenFileIsEmpty()
    {
        Assert.Throws<InvalidFileException>(
            () => _gameService.GetNewGameState("../../../../Pacman/Games/Empty.txt"));
    }

    [Theory]
    [InlineData("../../../../Pacman/Games/NoPac.txt")]
    [InlineData("../../../../Pacman/Games/MultiplePac.txt")]
    public void GetNewGameState_ThrowsInvalidFileException_WhenThereIsNotExactlyOnePacSymbol(string filepath)
    {
        Assert.Throws<InvalidFileException>(
            () => _gameService.GetNewGameState(filepath));
    }

    [Fact]
    public void GetNewGameState_ReturnsGameStateWithCorrectWallCoordinates_WhenGivenTxtFile()
    {
        var expectedWallCoords = new[] {new Coordinate(2, 2), new Coordinate(0, 3)};
        var actualGameState = _gameService.GetNewGameState(TestGame);

        Assert.Equal(expectedWallCoords, actualGameState.Walls.Select(w => w.Coordinate));
    }

    [Fact]
    public void GetNewGameState_ReturnsGameStateWithCorrectRandomGhostCoordinate_WhenGivenTxtFile()
    {
        var expectedRandomGhostCoord = new Coordinate(1, 2);
        var actualGameState = _gameService.GetNewGameState(TestGame);
        var randomGhosts = actualGameState.Ghosts.Where(g => g.GetType() == typeof(RandomGhost));

        Assert.Contains(randomGhosts, g => g.Coordinate == expectedRandomGhostCoord);
    }

    [Fact]
    public void GetNewGameState_ReturnsGameStateWithCorrectGreedyGhostCoordinate_WhenGivenTxtFile()
    {
        var expectedGreedyGhostCoord = new Coordinate(0, 2);
        var actualGameState = _gameService.GetNewGameState(TestGame);
        var randomGhosts = actualGameState.Ghosts.Where(g => g.GetType() == typeof(GreedyGhost));

        Assert.Contains(randomGhosts, g => g.Coordinate == expectedGreedyGhostCoord);
    }

    [Fact]
    public void GetNewGameState_ReturnsGameStateWithCorrectPathFindingGhostCoordinate_WhenGivenTxtFile()
    {
        var expectedPathFindingGhostCoord = new Coordinate(1, 3);
        var actualGameState = _gameService.GetNewGameState(TestGame);
        var randomGhosts = actualGameState.Ghosts.Where(g => g.GetType() == typeof(PathFindingGhost));

        Assert.Contains(randomGhosts, g => g.Coordinate == expectedPathFindingGhostCoord);
    }

    [Fact]
    public void GetNewGameState_ReturnsGameStateWithCorrectPelletCoordinates_WhenGivenTxtFile()
    {
        var expectedPelletCoords = new[] {new Coordinate(0, 0), new Coordinate(1, 0), new Coordinate(2, 0)};
        var actualGameState = _gameService.GetNewGameState(TestGame);

        Assert.Equal(expectedPelletCoords, actualGameState.Pellets.Select(p => p.Coordinate));
    }

    [Fact]
    public void GetNextRoundGameState_ThrowsInvalidOperationException_WhenInvokedBeforeGetNewGameState()
    {
        Assert.Throws<InvalidOperationException>(() => _gameService.GetNextRoundGameState(It.IsAny<GameState>()));
    }
    
    [Fact]
    public void GetNextRoundGameState_ReturnsGameStateWithRoundIncrementedByOne()
    {
        var gameState = _gameService.GetNewGameState(TestGame);

        var nextGameState = _gameService.GetNextRoundGameState(gameState);

        Assert.Equal(gameState.Round + 1, nextGameState.Round);
    }

    [Fact]
    public void GetNextRoundGameState_ReturnsGameStateWithPacmanCoordinateReset()
    {
        var gameState = _gameService.GetNewGameState(TestGame);
        var expectedPacCoord = gameState.Pac.Coordinate;
        gameState = gameState with
        {
            Pac = gameState.Pac with {Coordinate = new Coordinate(2, 3)}
        };

        var nextGameState = _gameService.GetNextRoundGameState(gameState);

        Assert.Equal(expectedPacCoord, nextGameState.Pac.Coordinate);
    }

    [Fact]
    public void GetNextRoundGameState_ReturnsGameStateWithGhostCoordinateReset()
    {
        var gameState = _gameService.GetNewGameState(TestGame);
        var expectedGhostCoords = gameState.Ghosts.Select(g => g.Coordinate);
        var ghosts = new MovableEntity[]
        {
            new RandomGhost(new Coordinate(0, 0), It.IsAny<int>(), It.IsAny<ISelector<Coordinate>>()),
            new GreedyGhost(new Coordinate(0, 3), It.IsAny<int>()),
            new PathFindingGhost(new Coordinate(2, 3), It.IsAny<int>())
        };
        gameState = gameState with
        {
            Ghosts = ghosts
        };

        var nextGameState = _gameService.GetNextRoundGameState(gameState);

        Assert.Equal(expectedGhostCoords, nextGameState.Ghosts.Select(g => g.Coordinate).SkipLast(1));
    }

    [Theory]
    [InlineData(2, typeof(RandomGhost))]
    [InlineData(3, typeof(GreedyGhost))]
    [InlineData(4, typeof(PathFindingGhost))]
    public void GetNextRoundGameState_ReturnsGameStateWithAdditionalGhostBasedOnRoundNumber(int round, Type typeOfGhost)
    {
        var gameState = _gameService.GetNewGameState(TestGame) with {Round = round};
        var nextGameState = _gameService.GetNextRoundGameState(gameState);
        for (var _ = 1; _ < round - 1; _++)
            nextGameState = _gameService.GetNextRoundGameState(nextGameState);

        Assert.Equal(gameState.Ghosts.Count() + round - 1, nextGameState.Ghosts.Count());
        Assert.Equal(typeOfGhost, nextGameState.Ghosts.Last().GetType());
    }

    [Fact]
    public void GetNextRoundGameState_ReturnsGameStateWithPelletsReset()
    {
        var gameState = _gameService.GetNewGameState(TestGame);
        var expectedPelletCoords = gameState.Pellets.Select(p => p.Coordinate);
        gameState = gameState with {Pellets = Array.Empty<Pellet>()};

        var nextGameState = _gameService.GetNextRoundGameState(gameState);

        Assert.Equal(expectedPelletCoords, nextGameState.Pellets.Select(p => p.Coordinate));
    }
    
    [Fact]
    public void GetResetGameState_ThrowsInvalidOperationException_WhenInvokedBeforeGetNewGameState()
    {
        Assert.Throws<InvalidOperationException>(() => _gameService.GetResetGameState(It.IsAny<GameState>()));
    }

    [Fact]
    public void GetResetGameState_ReturnsGameStateWithPacmanLifeReducedByOne()
    {
        var gameState = _gameService.GetNewGameState(TestGame);
        var originalPacLife = gameState.Pac.Lives;

        var nextGameState = _gameService.GetResetGameState(gameState);
        
        Assert.Equal(originalPacLife - 1, nextGameState.Pac.Lives);
    }
    
    [Fact]
    public void GetResetGameState_()
    {
        var gameState = _gameService.GetNewGameState(TestGame) with
        {
            Pellets = new Pellet[] {new(new Coordinate(0, 0))}
        };
        var expectedPelletCoords = gameState.Pellets.Select(p => p.Coordinate);

        var nextGameState = _gameService.GetResetGameState(gameState);
        var actualPelletCoords = nextGameState.Pellets.Select(p => p.Coordinate);

        Assert.Equal(expectedPelletCoords, actualPelletCoords);
    }
}
